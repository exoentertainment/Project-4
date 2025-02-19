using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderMissile : MonoBehaviour
{
    [SerializeField] MissileSO missileSO;

    private GameObject target;
    private GameObject targetIcon;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, missileSO.duration);
        
        SearchForTarget();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if(target != null)
            transform.LookAt(target.transform);
        
        transform.position += transform.rotation * Vector3.forward * missileSO.speed * Time.deltaTime;
    }

    void AssignTargetIcon()
    {
        targetIcon = Instantiate(missileSO.targetIcon, target.transform.position, Quaternion.identity);

        //StartCoroutine(AssignTargetIconRoutine());
    }
    
    void SearchForTarget()
    {
        Collider[] possibleTargets = Physics.OverlapSphere(transform.position, missileSO.range, missileSO.targetLayers);


        if (possibleTargets.Length > 0)
        {
            List<Collider> targets = new List<Collider>();
            targets.AddRange(possibleTargets);

            for (int x = targets.Count - 1; x > -1; x--)
            {
                if (!CameraManager.Instance.ObjectInCameraView(targets[x].gameObject.transform))
                    targets.RemoveAt(x);
            }

            if (targets.Count > 0)
            {
                int randomTarget = UnityEngine.Random.Range(0, targets.Count);
                target = targets[randomTarget].gameObject;
            }
        }
    }
}
