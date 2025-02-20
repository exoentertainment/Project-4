using UnityEngine;

[CreateAssetMenu(fileName = "Missile SO", menuName = "Missile SO")]
public class MissileSO : ScriptableObject
{
    public float damage;
    public float speed;
    public float duration;
    public int range;
    public LayerMask targetLayers;
    public GameObject missilePrefab;
}
