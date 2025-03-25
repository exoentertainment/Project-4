using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyMovement : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private int obstacleDetectionRange;
    [SerializeField] private GameObject waypointMarker;

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
    
    [SerializeField] private int minWaypointDistance;
    [SerializeField] private int maxWaypointDistance;

    #endregion
    
    private void Start()
    {
        if (behavior == Behaviors.Patrol)
        {
            currentBehavior = Behaviors.Patrol;
            waypoints = new Vector3[numWaypoints];
            SetWaypoints();
            transform.LookAt(waypoints[currentWaypoint]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //MoveForward();
        BehaviorTree();
    }

    void BehaviorTree()
    {
        switch (currentBehavior)
        {
            case Behaviors.Patrol:
            {
                if(!isDetoured)
                    MoveToWaypoint();
                
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
        if(!CheckForObstacle(obstacleDetectionRange))
            transform.position += transform.forward * (enemySO.moveSpeed * Time.deltaTime);
    }

    bool CheckForObstacle(float detectionRange)
    {
         if (Physics.Raycast(raycastOrigin.position, transform.forward, out RaycastHit hit, detectionRange))
         {
             if (hit.collider != null)
             {
                 Debug.Log("Obstacle");
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
                Vector3 newWaypoint = (Random.insideUnitSphere * Random.Range(minWaypointDistance, maxWaypointDistance)) + transform.position;
                float distToLastWaypoint = Vector3.Distance(waypoints[i-1], newWaypoint);

                if (distToLastWaypoint > minDistanceBetweenWaypoints)
                {
                    Vector3 normalizedDirection = (newWaypoint - transform.position).normalized;
                    //if (!Physics.Raycast(raycastOrigin.position, normalizedDirection, out RaycastHit hit, distToPos))
                    waypoints[i] = newWaypoint;
                }
            }
            
            Instantiate(waypointMarker, waypoints[i], Quaternion.identity);
        }
    }

    void MoveToWaypoint()
    {
        if (!CheckForObstacle(obstacleDetectionRange) && !isDetoured)
        {
            transform.position += transform.forward * (enemySO.moveSpeed * Time.deltaTime);
        }
        else if (!CheckForObstacle(obstacleDetectionRange) && isDetoured)
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
