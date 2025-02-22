using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderMissile : MonoBehaviour
{
    [SerializeField] MissileSO missileSO;

    private GameObject target;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, missileSO.duration);
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

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
