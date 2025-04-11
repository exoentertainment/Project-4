using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Platform SO", menuName = "Weapon Platform SO")]
public class TurretSO : ScriptableObject
{
    #region --Phase 1 Variables--

    public GameObject weaponPrefab;
    public string weaponDescription;
    public int UIScale;

    #endregion
    
    #region --Attack Variables--

    public BaseProjectileSO projectileSO;
    public AudioClipSO fireSFX;
    public int damage;
    public float fireRate;
    public int cost;
    public float baseTrackingSpeed;
    public float barrelTrackingSpeed;
    public int targetLoiterTime;
    public float barrelFireDelay;
    public int minRange;
    public LayerMask targetLayers;

    [Range(-1, 0)]
    public float trackingErrorMin;
    
    [Range(0, 1)]
    public float trackingErrorMax;
    
    public float GetTrackingError()
    {
        return Random.Range(trackingErrorMin, trackingErrorMax);
    }
    
    #endregion

    #region --Health Variables--

    public int maxHealth;
    public float explosionDuration;
    public float explosionFrequency;
    public GameObject explosionPrefab;

    #endregion
}
