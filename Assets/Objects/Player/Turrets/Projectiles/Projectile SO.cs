using UnityEngine;

[CreateAssetMenu(fileName = "Projectile SO", menuName = "Projectile SO")]
public class ProjectileSO : ScriptableObject
{
    public float speed;
    public float damage;
    public int duration;
    public GameObject projectilePrefab;
    public GameObject dischargePrefab;
    public GameObject impactPrefab;
}
