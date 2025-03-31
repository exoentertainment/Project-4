using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Platform SO", menuName = "Weapon Platform SO")]
public class WeaponPlatformSO : ScriptableObject
{
    public int damage;
    public float fireRate;
    public int cost;
    public float baseTrackingSpeed;
    public float barrelTrackingSpeed;
    public int targetLoiterTime;
    public float barrelFireDelay;
    public float barrelRotateSpeed;
    public int UIScale;

    public string weaponDescription;
    public BaseProjectileSO projectileSO;
    public AudioClipSO fireSFX;
    public GameObject weaponPrefab;
}
