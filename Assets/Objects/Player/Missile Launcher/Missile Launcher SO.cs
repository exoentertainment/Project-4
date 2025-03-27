using UnityEngine;

[CreateAssetMenu(fileName = "Missile Launcher SO", menuName = "Missile Launcher SO")]
public class MissileLauncherSO : ScriptableObject
{
    public float fireRate;

    public MissileType targettingType;
    public MissileSO missileSO;

    public enum MissileType
    {
        SingleTarget,
        MultiTarget
    }
}
