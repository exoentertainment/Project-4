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
    
    Vector3[] waypoints;
    int currentWaypoint = 1;
    
    
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
        if(!CheckForObstacle())
            transform.position += transform.forward * enemySO.moveSpeed * Time.deltaTime;
    }

    bool CheckForObstacle()
    {
        //shoot ray
        //if (Physics.Raycast(transform.position, waypoints[currentWaypoint] - transform.position, out RaycastHit hit, 5))
        if (Physics.Raycast(raycastOrigin.position, transform.forward, out RaycastHit hit, 5))
        {
            if (hit.collider != null)
            {
                Debug.Log("Obstacle: " + hit.collider.name);
                
                return true;
            }
        }
        
        return false;
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
                Vector3 newWaypoint = (Random.insideUnitSphere * 200) - transform.position;
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
