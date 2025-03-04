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

    [SerializeField] private GameObject spawnPortalPrefab;
    [SerializeField] private float spawnPortalDuration;
    [SerializeField] private float spawnTime;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int numSpawns;

    #endregion

    float lastSpawnTime;

    private void Start()
    {
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (Time.time - lastSpawnTime > spawnTime)
        {
            lastSpawnTime = Time.time;
            Vector3 spawnPos = Random.onUnitSphere * collider.radius;
            Quaternion spawnRot = Quaternion.FromToRotation(spawnPos, Vector3.up);
  
            GameObject spawnPortal = Instantiate(spawnPortalPrefab, spawnPos, Quaternion.identity);
            spawnPortal.transform.LookAt(Vector3.zero);
            Destroy(spawnPortal, spawnPortalDuration);
        }
    }
}
