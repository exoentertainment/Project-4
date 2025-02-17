using UnityEngine;

[CreateAssetMenu(fileName = "Missile SO", menuName = "Missile SO")]
public class MissileSO : ScriptableObject
{
    public float damage;
    public float speed;
    public float duration;
    public LayerMask targetLayers;
    public GameObject targetIcon;
    public float targetIconDuration;
    public GameObject missilePrefab;
}
