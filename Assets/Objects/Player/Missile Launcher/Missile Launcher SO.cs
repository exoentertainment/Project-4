using UnityEngine;

[CreateAssetMenu(fileName = "Missile Launcher SO", menuName = "Missile Launcher SO")]
public class MissileLauncherSO : ScriptableObject
{
    public float fireRate;
    public int range;
    public MissileSO missileSO;
}
