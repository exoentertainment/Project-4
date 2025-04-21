using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserTurret : BaseTurret
{
    #region --Serialized Fields--

    [SerializeField] private float laserDuration;
    [SerializeField] private int laserRange;

    #endregion
    
    LineRenderer lineRenderer;
    
    void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, spawnPoints[0].transform.position);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (target != null)
        {
            if (target.activeSelf)
            {
                UpdateLineRenderer();
            }
        }
    }

    //Find the closest target going in order of target priority. If a suitable target cant be found in the first priority then check for targets in the next priority
    protected override void SearchForTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, laserRange, platformSO.projectileSO.targetLayers);
        
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
    
    protected override bool IsLoSClear(GameObject obj)
    {
        // if(Physics.Linecast(raycastOrigin.position, target.transform.position, out RaycastHit hit))
        if(Physics.Linecast(raycastOrigin.position, raycastOrigin.position + (raycastOrigin.transform.forward * laserRange), out RaycastHit hit))
        {
            if (hit.collider.gameObject == target)
            {
                lastTimeOnTarget = Time.time;
                return true;
            }
        }
        
        return false;
    }
    
    void UpdateLineRenderer()
    {
        float distanceToTarget = Vector3.Distance(spawnPoints[0].transform.position, target.transform.position);
        lineRenderer.SetPosition(1, spawnPoints[0].transform.position + (spawnPoints[0].transform.forward * distanceToTarget));
    }
    
    protected override IEnumerator FireRoutine()
    {
        lineRenderer.enabled = true;
        
        lastTimeOnTarget = Time.time;
        
        yield return new WaitForSeconds(laserDuration);
        
        lastFireTime = Time.time;
        lineRenderer.enabled = false;
    }
    
    protected override void CheckDistanceToTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget > laserRange)
            target = null;
    }
}
