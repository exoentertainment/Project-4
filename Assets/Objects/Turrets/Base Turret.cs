using System;
using System.Collections;
using System.Numerics;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BaseTurret : MonoBehaviour
{
    #region --Serialized Fields

    [SerializeField] TurretSO platformSO;
    [SerializeField] private MMAutoRotate[] barrels;

    [SerializeField] private Transform platformBase;
    [SerializeField] private Transform platformTurret;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform raycastOrigin;

    #endregion

    private GameObject target;
    private float lastFireTime;
    private float lastTimeOnTarget;
    
    float yCurrentRotation;
    float xCurrentRotation;
    
    private void Start()
    {
        lastFireTime = Time.time;
    }
    
    private void Update()
    {
        if (target != null)
        {
            Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * platformSO.projectileSO.range, Color.red);
            CheckDistanceToTarget();
            RotateTowardsTarget();
            Fire();
        }
        else
        {
            SearchForTarget();
        }
    }
    
    //Find the closest target going in order of target priority. If a suitable target cant be found in the first priority then check for targets in the next priority
    void SearchForTarget()
    {
        Debug.Log("Searching for target");
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, platformSO.projectileSO.range,
            platformSO.projectileSO.targetLayers);
        
        if (possibleTargets.Length > 0)
        {
            float closestEnemy = platformSO.projectileSO.range;

            for (int x = 0; x < possibleTargets.Length; x++)
            {
                float distanceToEnemy =
                    Vector3.Distance(possibleTargets[x].transform.position, transform.position);

                if (IsLoSClear(possibleTargets[x].gameObject))
                    if (distanceToEnemy < closestEnemy && distanceToEnemy > platformSO.minRange)
                    {
                        closestEnemy = distanceToEnemy;
                        target = possibleTargets[x].gameObject;
                    }
            }
        }
    }
    
    //Check if the passed target is within line-of-sight. If it is, then return true
    bool IsLoSClear(GameObject obj)
    {
        if (Physics.Raycast(raycastOrigin.position, obj.transform.position - raycastOrigin.position, out RaycastHit hit, platformSO.projectileSO.range))
        {
            if (hit.collider.gameObject == obj)
            {
                lastTimeOnTarget = Time.time;
                return true;
            }
        }
        
        return false;
    }
    
    //Rotate the base and barrel towards target
    void RotateTowardsTarget()
    {
        Vector3 targetVector = target.transform.position - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);
        targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0);
        float baseYRotation = targetRotation.eulerAngles.y;

        platformBase.rotation = Quaternion.SlerpUnclamped(platformBase.rotation, targetRotation, platformSO.baseTrackingSpeed * Time.deltaTime);
        
        targetVector = target.transform.position - platformTurret.transform.position;
        targetVector.Normalize();
        targetRotation = Quaternion.LookRotation(targetVector);
        
        if ((baseYRotation - platformBase.rotation.eulerAngles.y) < 20f)
            platformTurret.rotation = Quaternion.SlerpUnclamped(platformTurret.rotation, targetRotation, platformSO.barrelTrackingSpeed * Time.deltaTime);
    }
    
    void Fire()
    {
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * platformSO.projectileSO.range, out RaycastHit hit, platformSO.projectileSO.range))
        {
            if (hit.collider.gameObject == target)
            {
                if ((Time.time - lastFireTime) > platformSO.fireRate)
                    StartCoroutine(FireRoutine());
            }

            if ((Time.time - lastTimeOnTarget) >= platformSO.targetLoiterTime)
            {
                lastTimeOnTarget = Time.time;
                target = null;
            }
        }
    }
    
    IEnumerator FireRoutine()
    {
        lastTimeOnTarget = Time.time;
        lastFireTime = Time.time;

        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(platformSO.projectileSO.projectilePrefab, spawnPoint.position,
                platformTurret.transform.rotation);

            if (platformSO.projectileSO.dischargePrefab != null)
                Instantiate(platformSO.projectileSO.dischargePrefab, spawnPoint.position,
                    platformTurret.transform.rotation);
            
            yield return new WaitForSeconds(platformSO.barrelFireDelay);
        }
        
        if(AudioManager.instance != null)
            AudioManager.instance.PlaySound(platformSO.fireSFX);
    }
    
    void CheckDistanceToTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget < platformSO.minRange)
            target = null;
    }
}
