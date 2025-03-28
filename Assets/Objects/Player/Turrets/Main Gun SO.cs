using UnityEngine;

[CreateAssetMenu(fileName = "Main Gun SO", menuName = "Main Gun SO")]
public class MainGunSO : ScriptableObject
{
    public float fireRate;
    public BaseProjectileSO projectileSO;
}
