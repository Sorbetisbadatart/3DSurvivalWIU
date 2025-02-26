using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private List<GameObject> PlayerWaypoints;

    private GameObject _enemy = null;


    [SerializeField] private float playerSafeDistance;
    [SerializeField] private float enemySpawnDistance;

    private bool canSpawn = true;


    // Start is called before the first frame update
    void Start()
    {
        //give estimate of player's location
        for (int i = 0; i < PlayerWaypoints.Count - 1; i++)
         EnemyPrefab.GetComponent<EnemyController>()._waypoints[i] = PlayerWaypoints[i].transform;

    }

    private void Update()
    {
        if (TimeController.Timeinstance.TimePassedThisTime(0) && canSpawn)
        { 
            SpawnEnemy();
            canSpawn = false;
            TextManager.TextInstance.CreateText(new Vector3(200, 800, 1), "The beast has awoke", Color.red);
            AudioManager.Instance.PlaySFX("EnemyRoar");
        }

        if (TimeController.Timeinstance.TimePassedThisTime(7))
        {
            
            if (_enemy = GameObject.Find("Enemy1(Clone)"))
            {
                Destroy(_enemy);
                TextManager.TextInstance.CreateText(new Vector3(200, 800, 1), "The beast now rests", Color.white);
            }
        }

        if (TimeController.Timeinstance.TimePassedThisTime(23) && !canSpawn)
        {
            TextManager.TextInstance.CreateText(new Vector3(200, 800, 1), "Better hide now", Color.red);
            canSpawn = true;
        }
    }
    private void SpawnEnemy()
    {
        Instantiate(EnemyPrefab, CheckEnemyCanSpawnInRadius(enemySpawnDistance, playerSafeDistance, PlayerPrefab.transform.position, 5), Quaternion.identity);
    }

    private Vector3 CheckEnemyCanSpawnInRadius(float EnemySpawnRadius, float PlayerSafeDistance, Vector3 center, float height)
    {
        //initialise
        Vector3 EnemySpawnLocation = new Vector3(0, 10, 0);
        int breakCheck = 10;
        Vector2 pointOnCircle = new Vector2(0, 0);

        while (breakCheck > 0)
        {
            pointOnCircle = Random.insideUnitCircle.normalized * EnemySpawnRadius;
            EnemySpawnLocation = new Vector3(pointOnCircle.x, height, pointOnCircle.y) + center;

            if (Vector3.Distance(EnemySpawnLocation,PlayerPrefab.transform.position) > PlayerSafeDistance)
            {
                Debug.Log("Found Spawn Location");
                break;
            }
            breakCheck--;
            Debug.Log("Too close!");
        }

        //return a random point of a circle in (x,z) with player as the centre
        return EnemySpawnLocation;
    }


}
