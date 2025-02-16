using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMainGun : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] private MainGunSO gunSO;
    [SerializeField] Transform convergencePoint;

    #endregion

    float lastFireTime;
    private bool isFiring;
    
    private void Start()
    {
        transform.LookAt(convergencePoint);
        lastFireTime = Time.time;
    }

    private void Update()
    {
        Fire();
    }

    public void ActivateGun()
    {
        isFiring = true;
    }

    public void DeactivateGun()
    {
        isFiring = false;
    }
    
    public void Fire()
    {
        if(isFiring)
            if ((Time.time - lastFireTime) > gunSO.fireRate)
            {
                lastFireTime = Time.time;
                
                Instantiate(gunSO.projectileSO.projectilePrefab, transform.position, transform.rotation);
            }
    }
}
