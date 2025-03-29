using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class WeaponPlatformTurretAttack : MonoBehaviour, IPlatformInterface
{
    #region --Serialized Fields

    [SerializeField] WeaponPlatformSO platformSO;

    [SerializeField] private Transform platformBase;
    [SerializeField] private Transform platformTurret;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform raycastOrigin;

    [SerializeField] private float trackingSpeed;

    #endregion

    private GameObject target;
    private float lastFireTime;

    private float xCurrentRotation;
    float yCurrentRotation;

    private void Start()
    {
        lastFireTime = Time.time;
    }

    private void Update()
    {
        if(target != null)
            Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * platformSO.projectileSO.range, Color.red);
        
        SearchForTarget();
        RotateTowardsTarget();
        Fire();
    }

    //Find the closest target going in order of target priority. If a suitable target cant be found in the first priority then check for targets in the next priority
    void SearchForTarget()
    {
        if (target == null)
        {
            for (int i = 0; i < platformSO.projectileSO.targetPriorities.Length; i++)
            {
                Collider[] possibleTargets = Physics.OverlapSphere(transform.position, platformSO.projectileSO.range,
                    platformSO.projectileSO.targetPriorities[i]);

                if (possibleTargets.Length > 0)
                {
                    float closestEnemy = platformSO.projectileSO.range;

                    for (int x = 0; x < possibleTargets.Length; x++)
                    {
                        float distanceToEnemy =
                            Vector3.Distance(possibleTargets[x].transform.position, transform.position);

                        if (IsLoSClear(possibleTargets[x].gameObject))
                            if (distanceToEnemy < closestEnemy)
                            {
                                closestEnemy = distanceToEnemy;
                                target = possibleTargets[x].gameObject;
                            }
                    }

                    if (target != null)
                        break;
                }
            }
        }
    }
    
    //Check if the passed target is within line-of-sight. If it is, then return true
    bool IsLoSClear(GameObject obj)
    {
        // if (Physics.Raycast(ray, out RaycastHit hit, platformSO.range))
        if (Physics.Raycast(raycastOrigin.position, obj.transform.position - raycastOrigin.position, out RaycastHit hit, platformSO.projectileSO.range))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    //Rotate the base and barrel towards target
    void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector3 targetVector = target.transform.position - transform.position;
            targetVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);
            float baseYRotation = targetRotation.eulerAngles.y;

            platformBase.rotation = Quaternion.Slerp(platformBase.rotation, targetRotation, platformSO.baseTrackingSpeed * Time.deltaTime);
            
            
            targetVector = target.transform.position - platformTurret.transform.position;
            targetVector.Normalize();
            targetRotation = Quaternion.LookRotation(targetVector);
            
            if ((baseYRotation - platformBase.rotation.eulerAngles.y) < 20f)
                platformTurret.rotation = Quaternion.Slerp(platformTurret.rotation, targetRotation, platformSO.barrelTrackingSpeed * Time.deltaTime);
        }
    }

    void Fire()
    {
        if (target != null)
        {
            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * platformSO.projectileSO.range, out RaycastHit hit,
                    platformSO.projectileSO.range))
            {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                        if ((Time.time - lastFireTime) > platformSO.fireRate)
                        {
                            lastFireTime = Time.time;

                            foreach (Transform spawnPoint in spawnPoints)
                            {
                                Instantiate(platformSO.projectileSO.projectilePrefab, spawnPoint.position, platformTurret.transform.rotation);
                    
                                if(platformSO.projectileSO.dischargePrefab !=null)
                                    Instantiate(platformSO.projectileSO.dischargePrefab, spawnPoint.position, platformTurret.transform.rotation);
                            }
                        }
            }
        }
    }

    //Disable this script. Called by the weapon manager
    public void TurnActivityOff()
    {
        this.enabled = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, platformSO.projectileSO.range);
    }
}
