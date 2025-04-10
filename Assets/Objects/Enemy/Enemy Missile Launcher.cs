using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EnemyMissileLauncher : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] MissileLauncherSO missileLauncherSO;
    [SerializeField] private Transform platformBase;
    [SerializeField] private Transform platformTurret;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] Transform raycastOrigin;

    #endregion
    
    float lastFireTime;
    private float lastTimeOnTarget;
    private GameObject target;

    private void Start()
    {
        lastFireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            RotateTowardsTarget();
            Fire();
            CheckDistanceToTarget();
        }
        else
        {
            SearchForTarget();
        }

        Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * missileLauncherSO.projectileSO.range);
    }
    
    void SearchForTarget()
    {
        if (target == null)
        {
            Collider[] possibleTargets = Physics.OverlapSphere(transform.position, missileLauncherSO.projectileSO.range, missileLauncherSO.projectileSO.targetLayers);
            
            if (possibleTargets.Length > 0)
            {
                float closestEnemy = missileLauncherSO.projectileSO.range;
    
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
            }
        }
    }
    
    //Check if the passed target is within line-of-sight. If it is, then return true
    bool IsLoSClear(GameObject obj)
    {
        if (Physics.Raycast(raycastOrigin.position, obj.transform.position - raycastOrigin.position, out RaycastHit hit, missileLauncherSO.projectileSO.range))
        {
            // if (hit.collider.gameObject.layer == LayerMask.NameToLayer(missileLauncherSO.projectileSO.targetLayerName))
            if (hit.collider.gameObject == obj)
            {
                // if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy Projectile"))
                // {
                //     lastTimeOnTarget = Time.time;
                //     return true;
                // }
                
                lastTimeOnTarget = Time.time;
                return true;
            }
        }
        
        return false;
    }
    
    void Fire()
    {
        if ((Time.time - lastFireTime) > missileLauncherSO.fireRate)
        {
            lastFireTime = Time.time;

            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * missileLauncherSO.projectileSO.range, out RaycastHit hit, missileLauncherSO.projectileSO.range))
            {
                if(hit.collider.gameObject == target)
                    StartCoroutine(FireRoutine());
                else
                {
                    if((Time.time - lastTimeOnTarget) >= missileLauncherSO.targetLoiterTime)
                    {
                        lastTimeOnTarget = Time.time;
                        target = null;
                    }
                }
            }
        }
    }
    
    IEnumerator FireRoutine()
    {
        lastTimeOnTarget = Time.time;

        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(missileLauncherSO.projectileSO.projectilePrefab, spawnPoint.position,
                platformTurret.transform.rotation);

            if (missileLauncherSO.projectileSO.dischargePrefab != null)
                Instantiate(missileLauncherSO.projectileSO.dischargePrefab, spawnPoint.position,
                    platformTurret.transform.rotation);
            
            yield return new WaitForSeconds(missileLauncherSO.barrelFireDelay);
        }
        
        //AudioManager.instance.PlaySound(missileLauncherSO.fireSFX);
    }

    //This is called if current missile selects a random target. Culls any nearby target that isn't on screen
    void SetRandomTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, missileLauncherSO.projectileSO.range, missileLauncherSO.projectileSO.targetLayers);
        
        if (possibleTargets.Length > 0)
        {
            int randomTarget;
            
            foreach (Transform spawnPoint in spawnPoints)
            {
                GameObject missile = Instantiate(missileLauncherSO.projectileSO.projectilePrefab, spawnPoint.position, transform.rotation);
                randomTarget = UnityEngine.Random.Range(0, possibleTargets.Length);
                missile.GetComponent<PlaceholderMissile>().SetTarget(possibleTargets[randomTarget].gameObject);
            }
        }
    }

    IEnumerator SetRandomTargetRoutine(Collider[] colliders)
    {
        bool targetFound = false;

        for(int i = 0; i < spawnPoints.Length; i++)
        {
            while (!targetFound)
            {
                int randomTarget = UnityEngine.Random.Range(0, colliders.Length);

                if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * missileLauncherSO.projectileSO.range, out RaycastHit hit, missileLauncherSO.projectileSO.range, missileLauncherSO.projectileSO.targetLayers))
                {
                    GameObject missile = Instantiate(missileLauncherSO.projectileSO.projectilePrefab, spawnPoints[i].position, transform.rotation);
                    missile.GetComponent<PlaceholderMissile>().SetTarget(colliders[randomTarget].gameObject);
                    //Debug.Log(colliders[randomTarget].gameObject.name);
                    target = colliders[randomTarget].gameObject;
                    targetFound = true;
                }
                else
                {
                    Debug.Log("No target collider found");
                }
                
                yield return null;
            }
            
            targetFound = false;
        }
    }

    //This is called if current missile selects closest target. Culls any nearby target that isn't on screen
    void SetSingleTarget()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject missile = Instantiate(missileLauncherSO.projectileSO.projectilePrefab, spawnPoint.position, transform.rotation);
            missile.GetComponent<PlaceholderMissile>().SetTarget(target);
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
            targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0);
            float baseYRotation = targetRotation.eulerAngles.y;

            platformBase.rotation = Quaternion.SlerpUnclamped(platformBase.rotation, targetRotation, missileLauncherSO.baseTrackingSpeed * Time.deltaTime);
            
            targetVector = target.transform.position - platformTurret.transform.position;
            targetVector.Normalize();
            targetRotation = Quaternion.LookRotation(targetVector);
            
            if ((baseYRotation - platformBase.rotation.eulerAngles.y) < 20f)
                platformTurret.rotation = Quaternion.SlerpUnclamped(platformTurret.rotation, targetRotation, missileLauncherSO.barrelTrackingSpeed * Time.deltaTime);
        }
    }
    
    void CheckDistanceToTarget()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget > missileLauncherSO.projectileSO.range)
                target = null;
        }
    }
}
