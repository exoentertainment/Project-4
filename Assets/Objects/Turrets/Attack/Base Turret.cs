using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class BaseTurret : MonoBehaviour
{
    #region --Serialized Fields

    [SerializeField] protected TurretSO platformSO;

    [SerializeField] protected  Transform platformBase;
    [SerializeField] protected  Transform platformTurret;
    [SerializeField] protected Transform[] spawnPoints;
    [SerializeField] protected Transform raycastOrigin;

    #endregion

    protected GameObject target;
    protected float lastFireTime;
    protected float lastTimeOnTarget;
    
    Vector2 currentRotation;
    protected float yCurrentRotation;
    protected float xCurrentRotation;
    
    protected ObjectPool projectilePool;

    protected void Awake()
    {
        projectilePool = GetComponent<ObjectPool>();
    }

    protected void Start()
    {
        lastFireTime = Time.time;
    }
    
    protected void Update()
    {
        if (target != null)
        {
            if (target.activeSelf)
            {
                Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * platformSO.projectileSO.range, Color.red);
                CheckDistanceToTarget();
                RotateTowardsTarget();
                Fire();
            }
            else
                target = null;
        }
        else
        {
            SearchForTarget();
        }
    }
    
    //Find the closest target going in order of target priority. If a suitable target cant be found in the first priority then check for targets in the next priority
    protected void SearchForTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, platformSO.projectileSO.range, platformSO.projectileSO.targetLayers);
        
        if (possibleTargets.Length > 0)
        {
            float closestEnemy = Mathf.Infinity;

            for (int x = 0; x < possibleTargets.Length; x++)
            {
                float distanceToEnemy =
                    Vector3.Distance(possibleTargets[x].transform.position, transform.position);

                //if (IsLoSClear(possibleTargets[x].gameObject))
                    if (distanceToEnemy < closestEnemy)
                    {
                        closestEnemy = distanceToEnemy;
                        target = possibleTargets[x].gameObject;
                    }
            }
        }
    }
    
    //Check if the passed target is within line-of-sight. If it is, then return true
    protected bool IsLoSClear(GameObject obj)
    {
        if (Physics.Raycast(raycastOrigin.position, target.transform.position - raycastOrigin.position, out RaycastHit hit, platformSO.projectileSO.range))
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
    protected void RotateTowardsTarget()
    {
        Vector3 targetVector = target.transform.position - transform.position;
        targetVector.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(targetVector);
        targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0);
        float baseYRotation = targetRotation.eulerAngles.y;

        platformBase.rotation = Quaternion.SlerpUnclamped(platformBase.rotation, targetRotation, platformSO.baseTrackingSpeed * Time.deltaTime);
        currentRotation.y = platformBase.rotation.eulerAngles.y;

        
        targetVector = target.transform.position - platformTurret.transform.position;
        targetVector.Normalize();
        targetRotation = Quaternion.LookRotation(targetVector);
        
        if ((baseYRotation - currentRotation.y) < 10f)
        {
            platformTurret.rotation = Quaternion.SlerpUnclamped(platformTurret.rotation, targetRotation, platformSO.barrelTrackingSpeed * Time.deltaTime);
        }
    }
    
    protected void Fire()
    {
        if(IsLoSClear(target))
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
    
    protected virtual IEnumerator FireRoutine()
    {
        lastTimeOnTarget = Time.time;
        lastFireTime = Time.time;
        
        Quaternion shootingAngle = new Quaternion();
        shootingAngle.eulerAngles = new Vector3(platformTurret.rotation.eulerAngles.x + platformSO.GetTrackingError(), platformTurret.rotation.eulerAngles.y + platformSO.GetTrackingError(), 0);

        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject projectile = projectilePool.GetPooledObject(); 
            if (projectile != null) {
                projectile.transform.position = spawnPoint.position;
                projectile.transform.rotation = shootingAngle;
                projectile.SetActive(true);
            }
            
            if (platformSO.projectileSO.dischargePrefab != null)
                Instantiate(platformSO.projectileSO.dischargePrefab, spawnPoint.position,
                    platformTurret.transform.rotation);
            
            yield return new WaitForSeconds(platformSO.barrelFireDelay);
        }
        
        if(AudioManager.instance != null)
            AudioManager.instance.PlaySound(platformSO.fireSFX);
    }
    
    protected void CheckDistanceToTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget > platformSO.projectileSO.range)
            target = null;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, platformSO.projectileSO.range);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, platformSO.minRange);
    }
}
