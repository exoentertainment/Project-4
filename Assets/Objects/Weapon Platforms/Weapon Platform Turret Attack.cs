using System;
using UnityEngine;

public class WeaponPlatformTurretAttack : MonoBehaviour
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
    
    bool IsLoSClear(GameObject obj)
    {
        Ray ray = new Ray(transform.position, obj.transform.position - transform.position);

        if (Physics.Raycast(ray, out RaycastHit hit, platformSO.range))
        {
            if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, (obj.transform.position - transform.position) * 500, Color.red); 
                
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    return true;
            }
        }
        
        return true;
    }
    
    void IsTargetStillInView()
    {
        if (target != null)
        {
            Ray ray = new Ray(transform.position, target.transform.position - transform.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null)
                {
                    Debug.DrawRay(transform.position, (target.transform.position - transform.position) * 500, Color.red); 
                
                    if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
                        target = null;
                }
            }
        }
    }

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
}
