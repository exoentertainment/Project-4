using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Platform SO", menuName = "Weapon Platform SO")]
public class TurretSO : ScriptableObject
{
    public int damage;
    public float fireRate;
    public int cost;
    public float baseTrackingSpeed;
    public float barrelTrackingSpeed;
    public int targetLoiterTime;
    public float barrelFireDelay;
    public int minRange;
    public int UIScale;
    
    [Range(-1, 0)]
    public float trackingErrorMin;
    
    [Range(0, 1)]
    public float trackingErrorMax;
    
    public LayerMask[] targetPriorities;
    public LayerMask targetLayers;
    
    public string weaponDescription;
    public BaseProjectileSO projectileSO;
    public AudioClipSO fireSFX;
    public GameObject weaponPrefab;
    
    public float GetTrackingError()
    {
        return Random.Range(trackingErrorMin, trackingErrorMax);
    }
}
