using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemyMovement : MonoBehaviour
{
    #region --Old Code--

        // #region --Serialized Fields--
    //
    // [SerializeField] private EnemySO enemySO;
    // [SerializeField] private Transform raycastOrigin;
    // [SerializeField] private int obstacleDetectionRange;
    // [SerializeField] private GameObject waypointMarker;
    // [SerializeField] GameObject detourMarker;
    //
    // [Header("Behavior Settings")] 
    // [SerializeField] Behaviors behavior;
    //
    // [Header("Patrol Behavior Settings")]
    // [SerializeField] int numWaypoints;
    // //[SerializeField] float minDistanceBetweenWaypoints;
    // [SerializeField] private float waypointContactDistance;
    //
    // #endregion
    //
    // enum Behaviors
    // {
    //     Nothing,
    //     Patrol,
    //     Chase,
    //     Flyby,
    //     StopAtDistance
    // }
    // Behaviors currentBehavior;
    //
    // #region --Patrol Variables--
    //
    // Vector3[] waypoints;
    // int currentWaypoint = 1;
    // private bool isDetoured;
    // private Vector3 detourPos;
    //
    // [SerializeField] private int minWaypointDistance;
    // [SerializeField] private int maxWaypointDistance;
    //
    // #endregion
    //
    // private void Start()
    // {
    //     if (behavior == Behaviors.Patrol)
    //     {
    //         currentBehavior = Behaviors.Patrol;
    //         waypoints = new Vector3[numWaypoints];
    //         SetWaypoints();
    //         
    //     }
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     //MoveForward();
    //     BehaviorTree();
    // }
    //
    // void BehaviorTree()
    // {
    //     switch (currentBehavior)
    //     {
    //         case Behaviors.Patrol:
    //         {
    //             MoveToWaypoint();
    //             
    //             break;
    //         }
    //
    //         case Behaviors.Chase:
    //         {
    //             
    //             
    //             break;
    //         }
    //
    //         case Behaviors.Flyby:
    //         {
    //             
    //             
    //             break;
    //         }
    //
    //         case Behaviors.StopAtDistance:
    //         {
    //             
    //             
    //             break;
    //         }
    //     }
    // }
    //
    // void MoveForward()
    // {
    //     if(!CheckForObstacle(obstacleDetectionRange))
    //         transform.position += transform.forward * (enemySO.moveSpeed * Time.deltaTime);
    // }
    //
    // bool CheckForObstacle(float detectionRange)
    // {
    //      if (Physics.Raycast(raycastOrigin.position, transform.forward, out RaycastHit hit, detectionRange))
    //      {
    //          if (hit.collider != null)
    //          {
    //              isDetoured = true;
    //              //SetDetourPosition(hit);
    //             
    //              return true;
    //          }
    //      }
    //     
    //      return false;
    // }
    //
    // void SetDetourPosition(RaycastHit obstacle)
    // {
    //     Vector3 newPos = (Random.onUnitSphere * (obstacle.collider.bounds.extents.z * 4)) + obstacle.transform.position;
    //     float distToPos = Vector3.Distance(transform.position, newPos);
    //     Vector3 normalizedDirection = (newPos - transform.position).normalized;
    //     
    //     if (!Physics.Raycast(raycastOrigin.position, normalizedDirection, out RaycastHit hit, distToPos))
    //     {
    //         detourPos = newPos;
    //         Instantiate(detourMarker, detourPos, Quaternion.identity);
    //         transform.LookAt(detourPos);
    //     }
    // }
    //
    // #region --Patrol Logic--
    //
    // void SetWaypoints()
    // {
    //     StartCoroutine(SetNewWaypointRoutine());
    //     
    //     // for (int i = 0; i < numWaypoints; ++i)
    //     // {
    //     //     if (i == 0)
    //     //     {
    //     //         waypoints[i] = transform.position;
    //     //     }
    //     //     else
    //     //     {
    //     //         StartCoroutine(SetNewWaypointRoutine(i));
    //     //         
    //     //     //     Vector3 newWaypoint = (Random.insideUnitSphere * Random.Range(minWaypointDistance, maxWaypointDistance)) + transform.position;
    //     //     //     float distToLastWaypoint = Vector3.Distance(waypoints[i-1], newWaypoint);
    //     //     //
    //     //     //     if (distToLastWaypoint > minDistanceBetweenWaypoints)
    //     //     //     {
    //     //     //         Vector3 normalizedDirection = (newWaypoint - transform.position).normalized;
    //     //     //         //if (!Physics.Raycast(raycastOrigin.position, normalizedDirection, out RaycastHit hit, distToPos))
    //     //     //         waypoints[i] = newWaypoint;
    //     //     //     }
    //     //     }
    //     // }
    //     
    //     
    // }
    //
    // IEnumerator SetNewWaypointRoutine()
    // {
    //     bool waypointFound;
    //
    //     for (int i = 0; i < numWaypoints; ++i)
    //     {
    //         waypointFound = false;
    //         
    //         if (i == 0)
    //         {
    //             waypoints[i] = transform.position;
    //         }
    //         else
    //         {
    //             while (!waypointFound)
    //             {
    //                 //Vector3 newWaypoint = (Random.insideUnitSphere * Random.Range(minWaypointDistance, maxWaypointDistance)) + transform.position;
    //                 Vector3 newWaypoint = (Random.insideUnitSphere * Random.Range(minWaypointDistance, maxWaypointDistance));
    //                 
    //                 Vector3 normalizedDirection = (newWaypoint - waypoints[i - 1]).normalized;
    //                 if (!Physics.Raycast(waypoints[i - 1], normalizedDirection, out RaycastHit hit, maxWaypointDistance))
    //                 {
    //                     waypointFound = true;
    //                     waypoints[i] = newWaypoint;
    //                     Instantiate(waypointMarker, waypoints[i], Quaternion.identity);
    //                     Debug.Log("Waypoint selected: " + i);
    //                 }
    //         
    //                 yield return new WaitForEndOfFrame();
    //             }
    //         }
    //     }
    //     
    //     // while (!waypointFound)
    //     // {
    //     //     Vector3 newWaypoint = (Random.insideUnitSphere * Random.Range(minWaypointDistance, maxWaypointDistance)) + transform.position;
    //     //     float distToLastWaypoint = Vector3.Distance(waypoints[waypointIndex - 1], newWaypoint);
    //     //     
    //     //     if (distToLastWaypoint > minDistanceBetweenWaypoints)
    //     //     {
    //     //         Vector3 normalizedDirection = (newWaypoint - waypoints[waypointIndex - 1]).normalized;
    //     //         if (!Physics.Raycast(waypoints[waypointIndex - 1], normalizedDirection, out RaycastHit hit, maxWaypointDistance))
    //     //         {
    //     //             waypointFound = true;
    //     //             waypoints[waypointIndex] = newWaypoint;
    //     //             Instantiate(waypointMarker, waypoints[waypointIndex], Quaternion.identity);
    //     //             Debug.Log("Waypoint selected: " + waypointIndex);
    //     //         }
    //     //     }
    //         
    //         // yield return new WaitForEndOfFrame();
    //     //}
    //     
    //     transform.LookAt(waypoints[1]);
    // }
    //
    // void MoveToWaypoint()
    // {
    //     if (!CheckForObstacle(obstacleDetectionRange))
    //     {
    //         transform.position += transform.forward * (enemySO.moveSpeed * Time.deltaTime);
    //     }
    //
    //     float distToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypoint]);
    //     Debug.Log(distToWaypoint);
    //     if(distToWaypoint < waypointContactDistance)
    //         SelectNextWaypoint();
    // }
    //
    // void IsWaypointPathClear()
    // {
    //     Vector3 normalizedDirection = (waypoints[currentWaypoint] - transform.position).normalized;
    //     float distToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypoint]);
    //     
    //     if (!Physics.Raycast(raycastOrigin.position, normalizedDirection, out RaycastHit hit, distToWaypoint))
    //     {
    //         isDetoured = false;
    //         transform.LookAt(waypoints[currentWaypoint]);
    //     }
    // }
    //
    // void SelectNextWaypoint()
    // {
    //     currentWaypoint++;
    //     if(currentWaypoint == waypoints.Length)
    //         currentWaypoint = 0;
    //     
    //     transform.LookAt(waypoints[currentWaypoint]);
    // }
    //
    // #endregion

    #endregion
    
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private GameObject posMarker;
    [SerializeField] float rotateSpeed;
    [SerializeField] private float radius;

    Vector3 targetPos;
    
    private void Start()
    {
        StartCoroutine(SetNewPosition());
    }

    private void Update()
    {
        if(targetPos != null)
            Debug.DrawRay(raycastOrigin.position, targetPos - raycastOrigin.position, Color.red);
        
        MoveTowardsTarget();
        RotateTowardsTarget();
        CheckDistanceToTarget();
    }

    IEnumerator SetNewPosition()
    {
        Vector3 potentialPos;
        bool foundTarget = false;
        
        while (!foundTarget)
        {
            potentialPos = Random.onUnitSphere * radius;

            if (IsLoSClear(potentialPos))
            {
                Debug.Log("No obstacle found");
                Instantiate(posMarker, potentialPos, Quaternion.identity);
                targetPos = potentialPos;
                foundTarget = true;
            }
            else
            {
                Debug.Log("Obstacle found");
            }

            yield return new WaitForEndOfFrame();
        }

    }

    bool IsLoSClear(Vector3 pos)
    {
        if (!Physics.Raycast(raycastOrigin.position, pos - raycastOrigin.position, out RaycastHit hit))
        {
            return true;
        }
        
        return false;
    }

    void MoveTowardsTarget()
    {
        transform.position += transform.forward * (enemySO.moveSpeed * Time.deltaTime);
    }

    void RotateTowardsTarget()
    {
        if (targetPos != null)
        {
            Vector3 targetVector = targetPos - transform.position;
            targetVector.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(targetVector);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                rotateSpeed * Time.deltaTime);
        }
    }

    void CheckDistanceToTarget()
    {
        if (Vector3.Distance(transform.position, targetPos) <= 5)
        {
            StartCoroutine(SetNewPosition());
        }
    }
}
