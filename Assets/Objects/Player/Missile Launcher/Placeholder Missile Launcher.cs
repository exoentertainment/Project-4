using System;
using UnityEngine;

public class PlaceholderMissileLauncher : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] MissileLauncherSO missileLauncherSO;
    [SerializeField] private Transform[] spawnPoints;

    #endregion
    
    float lastFireTime;
    private bool isFiring;
    
    UITargetIcons uiTargetIcons;

    private void Awake()
    {
        uiTargetIcons = FindFirstObjectByType<UITargetIcons>();
    }

    private void Start()
    {
        lastFireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
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
    
    void Fire()
    {
        if(isFiring)
            if ((Time.time - lastFireTime) > missileLauncherSO.fireRate)
            {
                lastFireTime = Time.time;
                SetTargetIcons();
                
                foreach (Transform spawnPoint in spawnPoints)
                {
                    Instantiate(missileLauncherSO.missileSO.missilePrefab, spawnPoint.position, transform.rotation);
                }
            }
    }

    void SetTargetIcons()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, missileLauncherSO.missileSO.range, missileLauncherSO.missileSO.targetLayers);
        
        if(possibleTargets.Length > 0)
            uiTargetIcons.SetTargets(possibleTargets);
    }
}
