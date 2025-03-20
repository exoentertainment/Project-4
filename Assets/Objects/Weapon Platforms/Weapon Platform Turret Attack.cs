using System;
using UnityEngine;

public class WeaponPlatformTurretAttack : MonoBehaviour, IPlatformInterface
{
    #region --Serialized Fields

    [SerializeField] WeaponPlatformSO platformSO;

    [SerializeField] private Transform platformBase;
    [SerializeField] private Transform platformTurret;
    [SerializeField] Transform[] spawnPoints;

    #endregion

    private GameObject target;
    private float lastFireTime;

    private void Start()
    {
        lastFireTime = Time.time;
    }

    private void Update()
    {
        SearchForTarget();
        RotateTowardsTarget();
        Fire();
        
        IsTargetStillInView();
    }

    //Find the closest target going in order of target priority. If a suitable target cant be found in the first priority then check for targets in the next priority
    void SearchForTarget()
    {
        if(target == null)
            for (int i = 0; i < platformSO.targetPriorities.Length; i++)
            {
                Collider[] possibleTargets = Physics.OverlapSphere(transform.position, platformSO.range, platformSO.targetPriorities[i]);

                if (possibleTargets.Length > 0)
                {
                    float closestEnemy = Mathf.Infinity;

                    for (int x = 0; x < possibleTargets.Length; x++)
                    {
                        float distanceToEnemy =
                            Vector3.Distance(possibleTargets[x].transform.position, transform.position);

                        if(IsLoSClear(possibleTargets[x].gameObject))
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
    
    //Check if the passed target is within line-of-sight. If it is, then return true
    bool IsLoSClear(GameObject obj)
    {
        // if (Physics.Raycast(ray, out RaycastHit hit, platformSO.range))
        if (Physics.Raycast(spawnPoints[0].transform.position, obj.transform.position - spawnPoints[0].transform.position, out RaycastHit hit, platformSO.range))
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
    
    //Check if the passed target is within line-of-sight. If it isn't then set target to null so a new target can be found
    void IsTargetStillInView()
    {
        if (target != null)
        {
            if (Physics.Raycast(spawnPoints[0].transform.position, target.transform.position - spawnPoints[0].transform.position, out RaycastHit hit, platformSO.range))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                        target = null;
                }
            }
        }
    }

    //Rotate the base and barrel towards target
    void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector3 targetVector = target.transform.position - transform.position;
            targetVector.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(targetVector);

            platformBase.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            
            platformTurret.transform.LookAt(target.transform.position);
        }
    }

    void Fire()
    {
        if (target != null)
        {
            if ((Time.time - lastFireTime) > platformSO.fireRate)
            {
                lastFireTime = Time.time;

                foreach (Transform spawnPoint in spawnPoints)
                {
                    Instantiate(platformSO.projectilePrefab, spawnPoint.position, platformTurret.transform.rotation);
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
        Gizmos.DrawWireSphere(transform.position, platformSO.range);
    }
}
