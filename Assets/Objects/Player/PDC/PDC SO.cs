using UnityEngine;

[CreateAssetMenu(fileName = "PDC SO", menuName = "PDC SO")]
public class PDCSO : ScriptableObject
{
    public float RoF;
    public float range;
    
    [Range(-1, 0)]
    public float trackingErrorMin;
    
    [Range(0, 1)]
    public float trackingErrorMax;
    
    public float baseTrackingSpeed;
    public float barrelTrackingSpeed;
    public LayerMask targetMask;
    public GameObject projectilePrefab;
    
    public float GetTrackingError()
    {
        return Random.Range(trackingErrorMin, trackingErrorMax);
    }
}
