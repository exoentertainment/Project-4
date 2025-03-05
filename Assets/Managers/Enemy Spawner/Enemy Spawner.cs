using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class EnemySpawner : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] private SphereCollider collider;

    // [SerializeField] private GameObject spawnPortalPrefab;
    // [SerializeField] private float spawnPortalDuration;
    // [SerializeField] private float spawnTime;
    // [SerializeField] private GameObject enemyPrefab;
    // [SerializeField] private int numSpawns;

    [SerializeField] EnemyWaveSO[] waveSO;
    
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
        lastSpawnTimes = new float[waveSO.Length];
        
        for(int i = 0; i < lastSpawnTimes.Length; i++)
            lastSpawnTimes[i] = Time.time;
    }

    void SetNumSpawns()
    {
        numSpawns = new int[waveSO.Length];
        
        for(int i = 0; i < numSpawns.Length; i++)
            numSpawns[i] = waveSO[i].numSpawns;
    }
    
    private void Update()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < waveSO.Length; i++)
        {
            if (numSpawns[i] > 0)
            {
                if (Time.time - lastSpawnTimes[i] > waveSO[i].spawnFrequency)
                {
                    lastSpawnTimes[i] = Time.time;
                    numSpawns[i]--;

                    Vector3 spawnPos = Random.onUnitSphere * collider.radius;
                    GameObject spawnPortal = Instantiate(waveSO[i].spawnPortalPrefab, spawnPos, Quaternion.identity);
                    spawnPortal.transform.LookAt(Vector3.zero);
                    Destroy(spawnPortal, waveSO[i].spawnPortalDuration);

                    Instantiate(waveSO[i].enemyPrefab[Random.Range(0, waveSO[i].enemyPrefab.Length)], spawnPos,
                        spawnPortal.transform.rotation);
                }
            }
        }
    }
}
