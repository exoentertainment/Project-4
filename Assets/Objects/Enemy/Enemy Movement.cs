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
                else
                    MoveAroundDetour();
                
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
            transform.position += transform.forward * enemySO.moveSpeed * Time.deltaTime;
    }

    bool CheckForObstacle()
    {
        //shoot ray
        //if (Physics.Raycast(transform.position, waypoints[currentWaypoint] - transform.position, out RaycastHit hit, 5))
        
        if (Physics.Raycast(raycastOrigin.position, transform.forward, out RaycastHit hit, 15))
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
        bool foundPosition = false;

        // while (!foundPosition)
        // {
            Vector3 newPos = (Random.onUnitSphere * (obstacle.collider.bounds.extents.z * 2)) + obstacle.transform.position;
            float distToPos = Vector3.Distance(transform.position, newPos);
            Debug.DrawLine(raycastOrigin.position, newPos, Color.red);
            
            if (Physics.Raycast(raycastOrigin.position, newPos - transform.position, out RaycastHit hit, distToPos))
            {
                if (hit.collider == null)
                {
                    Debug.Log("Found new position");
                    detourPos = newPos;
                    foundPosition = true;
                }
                else
                {
                    Debug.Log(hit.collider.name);
                }
            }
        //} 
    }

    void MoveToDetourPosition()
    {
        
    }
    
    void MoveAroundDetour()
    {
        
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
                
                Debug.Log(waypoints[i]);
            }
            
        }
    }

    void MoveToWaypoint()
    {
        transform.position += transform.forward * enemySO.moveSpeed * Time.deltaTime;
        
        float distToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypoint]);
        if(distToWaypoint < waypointContactDistance)
            SelectNextWaypoint();
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
