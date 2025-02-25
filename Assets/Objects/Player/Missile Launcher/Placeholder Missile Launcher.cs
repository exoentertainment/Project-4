using System;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderMissileLauncher : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] MissileLauncherSO missileLauncherSO;
    [SerializeField] private Transform[] spawnPoints;

    #endregion
    
    float lastFireTime;
    private bool isFiring = true;
    
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
                
                if(missileLauncherSO.targettingType == MissileLauncherSO.MissileType.MultiTarget)
                    SetRandomTarget();
                else if(missileLauncherSO.targettingType == MissileLauncherSO.MissileType.SingleTarget)
                    SetSingleTarget();
            }
    }

    //This is called if current missile selects a random target. Culls any nearby target that isn't on screen
    void SetRandomTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, missileLauncherSO.missileSO.range, missileLauncherSO.missileSO.targetLayers);
        
        if (possibleTargets.Length > 0)
        {
            List<Collider> targets = new List<Collider>();
            targets.AddRange(possibleTargets);

            for (int x = targets.Count - 1; x > -1; x--)
            {
                if (!CameraManager.Instance.ObjectInCameraView(targets[x].gameObject.transform))
                    targets.RemoveAt(x);
            }

            if (targets.Count > 0)
            {
                int randomTarget;
                
                foreach (Transform spawnPoint in spawnPoints)
                {
                    GameObject missile = Instantiate(missileLauncherSO.missileSO.missilePrefab, spawnPoint.position, transform.rotation);
                    randomTarget = UnityEngine.Random.Range(0, targets.Count);
                    missile.GetComponent<PlaceholderMissile>().SetTarget(targets[randomTarget].gameObject);
                    
                    SetTargetIcons(targets[randomTarget].gameObject);
                }
            }
        }
    }

    //This is called if current missile selects closest target. Culls any nearby target that isn't on screen
    void SetSingleTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, missileLauncherSO.missileSO.range, missileLauncherSO.missileSO.targetLayers);
        
        if (possibleTargets.Length > 0)
        {
            List<Collider> targets = new List<Collider>();
            targets.AddRange(possibleTargets);

            //Remove any potential targets that aren't onscreen
            for (int x = targets.Count - 1; x > -1; x--)
            {
                if (!CameraManager.Instance.ObjectInCameraView(targets[x].gameObject.transform))
                    targets.RemoveAt(x);
            }

            if (targets.Count > 0)
            {
                GameObject target = null;
                float closestEnemy = Mathf.Infinity;

                for (int x = 0; x < targets.Count; x++)
                {
                    float distanceToEnemy =
                        Vector3.Distance(targets[x].transform.position, transform.position);

                    if (distanceToEnemy < closestEnemy)
                    {
                        closestEnemy = distanceToEnemy;
                        target = targets[x].gameObject;
                    }
                }
                
                //SetTargetIcons(target);
                
                foreach (Transform spawnPoint in spawnPoints)
                {
                    GameObject missile = Instantiate(missileLauncherSO.missileSO.missilePrefab, spawnPoint.position, transform.rotation);
                    missile.GetComponent<PlaceholderMissile>().SetTarget(target);
                }
            }
        }
    }
    
    void SetTargetIcons(GameObject target)
    {
        // Collider[] possibleTargets = Physics.OverlapSphere(transform.position, missileLauncherSO.missileSO.range, missileLauncherSO.missileSO.targetLayers);
        //
        // if(possibleTargets.Length > 0)
        uiTargetIcons.SetTargetIcon(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, missileLauncherSO.missileSO.range);
    }
}
