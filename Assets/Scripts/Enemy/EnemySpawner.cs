using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject PlayerPrefab;

    [SerializeField] private float playerSafeDistance;
    [SerializeField] private float enemySpawnDistance;

    private bool canSpawn = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (TimeController.Timeinstance.TimePassedThisTime(0) && canSpawn)
        { 
            SpawnEnemy();
            canSpawn = false;
        }

        if (TimeController.Timeinstance.TimePassedThisTime(23) && !canSpawn)
        {
            SpawnEnemy();
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
