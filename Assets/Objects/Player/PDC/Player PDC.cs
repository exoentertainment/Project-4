using System;
using UnityEngine;
using UnityEngine.Profiling;

public class PlayerPDC : MonoBehaviour
{
    [SerializeField] private PDCSO pdcSO;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private Transform pdcBase;
    [SerializeField] private Transform pdcGunMount;
    [SerializeField] Transform raycastOrigin;

    private GameObject target;
    private float lastFireTime;
    
    ObjectPool projectilePool;

    private void Awake()
    {
        projectilePool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        lastFireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
            Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * pdcSO.range, Color.red);
        
        SearchForTarget();
        RotateTowardsTarget();
        Fire();
    }

    void SearchForTarget()
    {
        if(target != null && !target.activeSelf)
            target = null;
        
        if (target == null)
        {
            Collider[] possibleTargets = Physics.OverlapSphere(transform.position, pdcSO.range, pdcSO.targetMask);

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
            }
        }
    }

    bool IsLoSClear(GameObject obj)
    {
        if (Physics.Raycast(raycastOrigin.position, obj.transform.position - raycastOrigin.position, out RaycastHit hit, pdcSO.range))
        {
            if (hit.collider != null)
            {
                // if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                if (hit.collider.gameObject.CompareTag("Enemy"))
                    return true;
            }
        }
        
        return false;
    }

    //Rotate the base of the PDC and its barrels towards target
    void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector3 targetVector = target.transform.position - transform.position;
            targetVector.Normalize();
            
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);
            float baseYRotation = targetRotation.eulerAngles.y;

            pdcBase.rotation = Quaternion.Slerp(pdcBase.rotation, targetRotation, pdcSO.baseTrackingSpeed * Time.deltaTime);
            
            
            targetVector = target.transform.position - pdcGunMount.transform.position;
            targetVector.Normalize();
            
            targetRotation = Quaternion.LookRotation(targetVector);
            

            if ((baseYRotation - pdcBase.rotation.eulerAngles.y) < 20f)
                pdcGunMount.rotation = Quaternion.Slerp(pdcGunMount.rotation, targetRotation, pdcSO.barrelTrackingSpeed * Time.deltaTime);
        }
    }

    
    //Fires the PDC and introduces a small random offset to the rounds don't always go in the same direction. Helps to ensure they cover a wider area and heightens chance of hitting small targets
    void Fire()
    {
        if (target != null)
        {
            if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward * pdcSO.range, out RaycastHit hit,
                    pdcSO.range))
                if ((Time.time - lastFireTime) > pdcSO.RoF)
                {
                    lastFireTime = Time.time;
                    
                    Quaternion shootingAngle = new Quaternion();
                    shootingAngle.eulerAngles = new Vector3(pdcGunMount.rotation.eulerAngles.x + pdcSO.GetTrackingError(), pdcGunMount.rotation.eulerAngles.y + pdcSO.GetTrackingError(), 0);

                    foreach (Transform spawnPoint in spawnPoints)
                    {
                        GameObject projectile = projectilePool.GetPooledObject(); 
                        if (projectile != null) {
                            projectile.transform.position = spawnPoint.position;
                            projectile.transform.rotation = shootingAngle;
                            projectile.SetActive(true);
                        }
                    }
                }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pdcSO.range);
        
    }
}