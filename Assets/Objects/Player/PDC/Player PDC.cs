using UnityEngine;

public class PlayerPDC : MonoBehaviour
{
    [SerializeField] private PDCSO pdcSO;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private Transform pdcBase;
    [SerializeField] private Transform pdcGunMount;

    private GameObject target;
    
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

    void Fire()
    {
        if (target != null)
        {
            
        }
    }
}
