using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Health SO", menuName = "Enemy Health SO")]
public class EnemyHealthSO : ScriptableObject
{
    public int maxHealth;
    
    public GameObject explosionPrefab;
    public int explosionDuration;
    public float explosionFrequency;
    
    public GameObject reactorExplosionPrefab;
}
