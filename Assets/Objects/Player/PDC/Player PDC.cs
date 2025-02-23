using System;
using UnityEngine;
using UnityEngine.Profiling;

public class PlayerPDC : MonoBehaviour
{
    [SerializeField] private PDCSO pdcSO;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private Transform pdcBase;
    [SerializeField] private Transform pdcGunMount;

    private GameObject target;
    private float lastFireTime;

    private void Start()
    {
        lastFireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        SearchForTarget();
        RotateTowardsTarget();
        Fire();

        IsTargetStillInView();
    }

    void SearchForTarget()
    {
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
        Ray ray = new Ray(transform.position, obj.transform.position - transform.position);

        if (Physics.Raycast(ray, out RaycastHit hit, pdcSO.range))
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

    //Rotate the base of the PDC and its barrels towards target
    void RotateTowardsTarget()
    {
        if (target != null)
        {
            Vector3 targetVector = target.transform.position - transform.position;
            targetVector.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(targetVector);

            pdcBase.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            
            pdcGunMount.transform.LookAt(target.transform.position);
        }
    }

    
    //Fires the PDC and introduces a small random offset to the rounds don't always go in the same direction. Helps to ensure they cover a wider area and heightens chance of hitting small targets
    void Fire()
    {
        if (target != null)
        {
            if ((Time.time - lastFireTime) > pdcSO.RoF)
            {
                lastFireTime = Time.time;
                
                Quaternion shootingAngle = new Quaternion();
                shootingAngle.eulerAngles = new Vector3(pdcGunMount.rotation.eulerAngles.x + pdcSO.GetTrackingError(), pdcGunMount.rotation.eulerAngles.y + pdcSO.GetTrackingError(), 0);

                foreach (Transform spawnPoint in spawnPoints)
                {
                    //Instantiate(pdcSO.projectilePrefab, spawnPoint.position, shootingAngle);
                    GameObject projectile = PDCPool.SharedInstance.GetPooledObject(); 
                    if (projectile != null) {
                        projectile.transform.position = spawnPoint.position;
                        projectile.transform.rotation = shootingAngle;
                        projectile.SetActive(true);
                    }
                }
                
            }
        }
    }
}