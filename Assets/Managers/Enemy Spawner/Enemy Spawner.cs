using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class EnemySpawner : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] private SphereCollider collider;

    [FormerlySerializedAs("waveSO")] [SerializeField] EnemyWaveSO[] enemySpawnManagers;
    
    #endregion

    float[] lastSpawnTimes;
    private int[] numSpawns;

    private void Start()
    {
        SetLastSpawnTimes();
        SetNumSpawns();
    }

    void SetLastSpawnTimes()
    {
        lastSpawnTimes = new float[enemySpawnManagers.Length];
        
        for(int i = 0; i < lastSpawnTimes.Length; i++)
            lastSpawnTimes[i] = Time.time;
    }

    void SetNumSpawns()
    {
        numSpawns = new int[enemySpawnManagers.Length];
        
        for(int i = 0; i < numSpawns.Length; i++)
            numSpawns[i] = enemySpawnManagers[i].numSpawns;
    }
    
    private void Update()
    {
        SpawnEnemyPortal();
    }

    //Go through each spawn manager and spawn an enemy from the manager if the spawn time has been reached. Each enemy and spawn portal is spawned on a random spot on the sphere collider
    void SpawnEnemyPortal()
    {
        for (int i = 0; i < enemySpawnManagers.Length; i++)
        {
            if (numSpawns[i] > 0)
            {
                if (Time.time - lastSpawnTimes[i] > enemySpawnManagers[i].spawnFrequency)
                {
                    lastSpawnTimes[i] = Time.time;
                    numSpawns[i]--;

                    Vector3 spawnPos = Random.onUnitSphere * collider.radius;
                    GameObject spawnPortal = Instantiate(enemySpawnManagers[i].spawnPortalPrefab, spawnPos, Quaternion.identity);
                    spawnPortal.transform.LookAt(Vector3.zero);
                    Destroy(spawnPortal, enemySpawnManagers[i].spawnPortalDuration);

                    StartCoroutine(SpawnEnemyRoutine(i, spawnPos, spawnPortal));
                }
            }
        }
    }

    IEnumerator SpawnEnemyRoutine(int i, Vector3 spawnPos, GameObject spawnPortal)
    {
        yield return new WaitForSeconds(enemySpawnManagers[i].spawnDelay);
        
        Instantiate(enemySpawnManagers[i].enemyPrefab[Random.Range(0, enemySpawnManagers[i].enemyPrefab.Length)], spawnPos,
            spawnPortal.transform.rotation);
    }
}
