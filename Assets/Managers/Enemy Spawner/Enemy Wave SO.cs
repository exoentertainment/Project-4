using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Wave SO", menuName = "Enemy Wave SO")]
public class EnemyWaveSO : ScriptableObject
{
    public GameObject spawnPortalPrefab;
    public float spawnPortalDuration;
    public float spawnFrequency;
    public float spawnDelay;
    public GameObject[] enemyPrefab;
    public int numSpawns;
}
