using System;
using UnityEngine;
using UnityEngine.Events;

public class FindTarget : MonoBehaviour
{
    [SerializeField] TurretSO platformSO;
    [SerializeField] UnityEvent<GameObject> onTargetFound;
    
    GameObject target;

    private void Update()
    {
        if (target != null)
        {
            if (target.activeSelf)
            {
                CheckDistanceToTarget();
            }
            else
                target = null;
        }
        else
            SearchForTarget();
    }

    protected virtual void SearchForTarget()
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
                    target = possibleTargets[x].transform.root.gameObject;
                }
            }
        }

        if (target != null)
            onTargetFound?.Invoke(target);
    }
    
    protected virtual void CheckDistanceToTarget()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget > platformSO.projectileSO.range)
            {
                target = null;
                onTargetFound?.Invoke(null);
            }
        }
    }
}
