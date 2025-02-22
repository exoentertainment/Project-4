using UnityEngine;

[CreateAssetMenu(fileName = "PDC SO", menuName = "PDC SO")]
public class PDCSO : ScriptableObject
{
    public float RoF;
    public float range;
    public float fireOffset;
    public LayerMask targetMask;
    public GameObject projectilePrefab;
}
