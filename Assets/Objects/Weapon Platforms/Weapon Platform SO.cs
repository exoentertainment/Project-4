using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Platform SO", menuName = "Weapon Platform SO")]
public class WeaponPlatformSO : ScriptableObject
{
    public int damage;
    public int range;
    public float fireRate;
    public int cost;
    public LayerMask[] targetPriorities;
    public int UIScale;

    public string weaponDescription;
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
}
