using System;
using UnityEngine;

public class WeaponPlatformSpot : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] private Transform weaponBuildPos;
    [SerializeField] private Material slotOccupiedColor;

    #endregion

    private GameObject weapon;

    //Places the passed weapon prefab at the build position and assigns itself as parent
    public void PlaceWeapon(GameObject obj)
    {
        GetComponent<MeshRenderer>().material = slotOccupiedColor;
        
        if(weapon != null)
            Destroy(weapon);
        
        weapon = Instantiate(obj, weaponBuildPos.position, Quaternion.identity);
        weapon.transform.parent = transform;
    }
}
