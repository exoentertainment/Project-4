using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyMovement : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Transform raycastOrigin;

    [Header("Behavior Settings")] 
    [SerializeField] Behaviors behavior;
    
    [Header("Patrol Behavior Settings")]
    [SerializeField] int numWaypoints;
    [SerializeField] float minDistanceBetweenWaypoints;
    [SerializeField] private float waypointContactDistance;
    
    #endregion

    enum Behaviors
    {
        Patrol,
        Chase,
        Flyby,
        StopAtDistance
    }
    Behaviors currentBehavior;

    #region --Patrol Variables--

    Vector3[] waypoints;
    int currentWaypoint = 1;
    private bool isDetoured;
    private Vector3 detourPos;

    #endregion
    
    private void Start()
    {
        // if (behavior == Behaviors.Patrol)
        // {
        //     currentBehavior = Behaviors.Patrol;
        //     waypoints = new Vector3[numWaypoints];
        //     SetWaypoints();
        //     transform.LookAt(waypoints[currentWaypoint]);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        //BehaviorTree();
    }

    void BehaviorTree()
    {
        switch (currentBehavior)
        {
            case Behaviors.Patrol:
            {
                if(!isDetoured)
                    MoveToWaypoint();
                // else
                //     MoveAroundDetour();
                
                break;
            }

            case Behaviors.Chase:
            {
                
                
                break;
            }

            case Behaviors.Flyby:
            {
                
                
                break;
            }

            case Behaviors.StopAtDistance:
            {
                
                
                break;
            }
        }
    }
    
    void MoveForward()
    {
        if(!CheckForObstacle())
            transform.position += transform.forward * (enemySO.moveSpeed * Time.deltaTime);
    }

    bool CheckForObstacle()
    {
         if (Physics.Raycast(raycastOrigin.position, transform.forward, out RaycastHit hit, 25))
         {
             if (hit.collider != null)
             {
                 isDetoured = true;
                 SetDetourPosition(hit);
                
                 return true;
             }
         }
        
         return false;
    }

    void SetDetourPosition(RaycastHit obstacle)
    {
        Vector3 newPos = (Random.onUnitSphere * (obstacle.collider.bounds.extents.z * 4)) + obstacle.transform.position;
        float distToPos = Vector3.Distance(transform.position, newPos);
        Vector3 normalizedDirection = (newPos - transform.position).normalized;
        
        if (!Physics.Raycast(raycastOrigin.position, normalizedDirection, out RaycastHit hit, distToPos))
        {
            detourPos = newPos;
            transform.LookAt(detourPos);
        }
    }
    
    #region --Patrol Logic--

    void SetWaypoints()
    {
        for (int i = 0; i < numWaypoints; ++i)
        {
            if (i == 0)
            {
                waypoints[i] = transform.position;
            }
            else
            {
                Vector3 newWaypoint = (Random.insideUnitSphere * 200) + transform.position;
                float distToLastWaypoint = Vector3.Distance(waypoints[i-1], newWaypoint);
                
                if(distToLastWaypoint > minDistanceBetweenWaypoints)
                    waypoints[i] = Random.onUnitSphere * 200;
            }
        }
    }

    void MoveToWaypoint()
    {
        if (!CheckForObstacle() && !isDetoured)
        {
            transform.position += transform.forward * (enemySO.moveSpeed * Time.deltaTime);
        }
        else if (!CheckForObstacle() && isDetoured)
        {
            transform.position += transform.forward * (enemySO.moveSpeed * Time.deltaTime);
            IsWaypointPathClear();
        }

        float distToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypoint]);
        if(distToWaypoint < waypointContactDistance)
            SelectNextWaypoint();
    }

    void IsWaypointPathClear()
    {
        Vector3 normalizedDirection = (waypoints[currentWaypoint] - transform.position).normalized;
        float distToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypoint]);
        
        if (!Physics.Raycast(raycastOrigin.position, normalizedDirection, out RaycastHit hit, distToWaypoint))
        {
            isDetoured = false;
            transform.LookAt(waypoints[currentWaypoint]);
        }
    }
    
    void SelectNextWaypoint()
    {
        currentWaypoint++;
        if(currentWaypoint == waypoints.Length)
            currentWaypoint = 0;
        
        transform.LookAt(waypoints[currentWaypoint]);
    }
    
    #endregion
}
