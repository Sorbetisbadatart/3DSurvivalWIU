﻿using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;


public class EndlessGeneration: MonoBehaviour {

	const float scale = .5f;

	const float viewerMoveThresholdForChunkUpdate = 10f;
	const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

	public LODInfo[] detailLevels;
	public static float maxViewDst;

    public Transform viewer;
	public Material mapMaterial;

	public static Vector2 viewerPosition;
	Vector2 viewerPositionOld;
	static MapGenerator mapGenerator;
	int chunkSize;
	int chunksVisibleInViewDst;

	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

	public GameObject waterPrefab;
	public GameObject cloudPrefab;
	public PlacementGenerator GenTerrain;


	
    void Start() {
		mapGenerator = FindObjectOfType<MapGenerator> ();

		maxViewDst = detailLevels [detailLevels.Length - 1].visibleDstThreshold;
		chunkSize = MapGenerator.mapChunkSize - 1;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);

		UpdateVisibleChunks ();
    }

	void Update() {
		
	


		viewerPosition = new Vector2 (viewer.position.x, viewer.position.z) / scale;
		
		if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
			viewerPositionOld = viewerPosition;
			UpdateVisibleChunks ();
		}
    }

    void UpdateVisibleChunks() {

		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++) terrainChunksVisibleLastUpdate[i].SetVisible(false);
		terrainChunksVisibleLastUpdate.Clear ();
			
		int currentChunkCoordX = Mathf.RoundToInt (viewerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt (viewerPosition.y / chunkSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) {
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
					terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
				else
					terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial, waterPrefab, cloudPrefab, GenTerrain));
            }
		}
	}
	public class TerrainChunk {

		GameObject meshObject;
		GameObject waterPrefab;
		GameObject cloudPrefab;
		PlacementGenerator TerrainGenPrefab;
		Vector2 position;
		Bounds bounds;

		MeshRenderer meshRenderer;
		MeshFilter meshFilter;
		MeshCollider meshCollider;
        NavMeshSurface navmeshSurface;


        LODInfo[] detailLevels;
		LODMesh[] lodMeshes;
		LODMesh collisionLODMesh;

		MapData mapData;
		bool mapDataReceived;
		int previousLODIndex = -1;

		bool baked = false;
        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material, GameObject waterPrefab, GameObject cloudPrefab, PlacementGenerator TerrainGen) {
			this.detailLevels = detailLevels;

			position = coord * size;
			bounds = new Bounds(position,Vector2.one * size);
			Vector3 positionV3 = new Vector3(position.x,0,position.y);

			meshObject = new GameObject("Terrain Chunk");
			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			meshFilter = meshObject.AddComponent<MeshFilter>();
			meshCollider = meshObject.AddComponent<MeshCollider>();
			navmeshSurface = meshObject.AddComponent<NavMeshSurface>();
			meshRenderer.material = material;

			meshObject.transform.position = positionV3 * scale;
			meshObject.transform.parent = parent;
			meshObject.transform.localScale = Vector3.one * scale;

			waterPrefab = Instantiate(waterPrefab);
            waterPrefab.transform.position = new Vector3(positionV3.x * scale,-3.6f,positionV3.z*scale);
            waterPrefab.transform.parent = parent;
			this.waterPrefab = waterPrefab; // For visibility

			cloudPrefab = Instantiate(cloudPrefab);
            cloudPrefab.transform.position = new Vector3(positionV3.x * scale,33,positionV3.z*scale);
            cloudPrefab.transform.parent = parent;
			this.cloudPrefab = cloudPrefab; // For visibility

            TerrainGen = Instantiate(TerrainGen);
            TerrainGen.transform.position = positionV3 * scale;
            TerrainGen.transform.parent = parent;
			TerrainGenPrefab = TerrainGen; // For visibility

            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
			for (int i = 0; i < detailLevels.Length; i++) {
				lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
				if (detailLevels[i].useForCollider) collisionLODMesh = lodMeshes[i];
			}
			mapGenerator.RequestMapData(position,OnMapDataReceived);
        }

        void OnMapDataReceived(MapData mapData) {
			this.mapData = mapData;
			mapDataReceived = true;

			Texture2D texture = TextureGenerator.TextureFromColourMap (mapData.colourMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
			meshRenderer.material.mainTexture = texture;

			UpdateTerrainChunk ();
		}

		public void UpdateTerrainChunk() {
           
            if (mapDataReceived) {
				float viewerDstFromNearestEdge = Mathf.Sqrt (bounds.SqrDistance (viewerPosition));
				bool visible = viewerDstFromNearestEdge <= maxViewDst;

				if (visible)
				{
					int lodIndex = 0;

					for (int i = 0; i < detailLevels.Length - 1; i++)
					{
						if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold)
						lodIndex = i + 1;
						else
						{
							break;
						}
					}

					if (lodIndex != previousLODIndex)
					{
						LODMesh lodMesh = lodMeshes[lodIndex];
						if (lodMesh.hasMesh)
						{
							previousLODIndex = lodIndex;
							meshFilter.mesh = lodMesh.mesh;
						}
						else if (!lodMesh.hasRequestedMesh)
							lodMesh.RequestMesh(mapData);
					}

					if (lodIndex == 0)
					{
						if (collisionLODMesh.hasMesh)
							meshCollider.sharedMesh = collisionLODMesh.mesh;
						else if (!collisionLODMesh.hasRequestedMesh)
							collisionLODMesh.RequestMesh(mapData);
					}

					terrainChunksVisibleLastUpdate.Add(this);
					if (!baked)
						BakeNavMesh();
				}
                SetVisible (visible);
			}
		}

		public void BakeNavMesh()
		{

			navmeshSurface.BuildNavMesh();
			baked = true;
		}

        public void SetVisible(bool visible) {
			meshObject.SetActive (visible);
			waterPrefab.SetActive (visible);
			cloudPrefab.SetActive (visible);
			TerrainGenPrefab.SetVisibility (!visible);
		}

		public bool IsVisible() {
			return meshObject.activeSelf;
		}

		private void ReplaceTerrain(){
			TerrainGenPrefab.Clear();
			TerrainGenPrefab.Generate();
		}

		private void DailyReset()
		{
			if (TimeController.Timeinstance.TimePassedThisTime(2))
			{
				ReplaceTerrain();
				Debug.Log("reste");
			}
		}
	}
	class LODMesh {
		public Mesh mesh;
		public bool hasRequestedMesh;
		public bool hasMesh;
		int lod;
		System.Action updateCallback;

		public LODMesh(int lod, System.Action updateCallback) {
			this.lod = lod;
			this.updateCallback = updateCallback;
		}

		void OnMeshDataReceived(MeshData meshData) {
			mesh = meshData.CreateMesh ();
			hasMesh = true;

			updateCallback ();
		}

		public void RequestMesh(MapData mapData) {
			hasRequestedMesh = true;
			mapGenerator.RequestMeshData (mapData, lod, OnMeshDataReceived);
		}
	}

	[System.Serializable]
	public struct LODInfo {
		public int lod;
		public float visibleDstThreshold;
		public bool useForCollider;
	}

}
