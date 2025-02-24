using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaceholderMissile : MonoBehaviour
{
    [Tooltip("The maximum amount of swarm.")]
    [SerializeField] protected float maxSwarmAmount = 20;

    [Tooltip("The distance from the target over which the swarm fades to zero (so that the missile can aim at the target as it gets close).")]
    [SerializeField] protected float swarmFadeDistance = 100;

    [Tooltip("The swarm frequency (how rapidly it weaves from one side to the other).")]
    [SerializeField] protected float swarmFrequency = 2;

    [Tooltip("The maximum amount of steering power (applied as a lerp) to guide the missile to the target.")]
    [SerializeField] protected float guidanceSteeringPower = 5;

    [Tooltip("The time from when the missile is launched to when the swarm level is zero (necessary to prevent the missile from sometimes getting stuck in a swarm behaviour that carries it away from the target).")]
    [SerializeField] protected float swarmFadeTime = 5;

    protected float startTime;
    protected float randomTimeOffset;
    
    [SerializeField] MissileSO missileSO;

    private GameObject target;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, missileSO.duration);
        randomTimeOffset = Random.Range(0f, 1000f);
        startTime = Time.time;
        
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
        
        float dist = Vector3.Distance(transform.position, target.transform.position);
        float swarmAmount = dist > swarmFadeDistance ? 1 : (dist / swarmFadeDistance);
        swarmAmount *= Mathf.Clamp(1 - ((Time.time - startTime) / swarmFadeTime), 0, 1);

        Vector3 fwd = transform.forward;
        Vector3 toTarget = (target.transform.position - transform.position).normalized;
        fwd = Vector3.Lerp(fwd, toTarget, (1 - swarmAmount) * guidanceSteeringPower * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(fwd, Vector3.up);

        float wiggleX = swarmAmount * (Mathf.PerlinNoise((Time.time + randomTimeOffset) * swarmFrequency, 0.2f) - 0.5f) * maxSwarmAmount;
        float wiggleY = swarmAmount * (Mathf.PerlinNoise((Time.time + randomTimeOffset) * swarmFrequency, 0.5f) - 0.5f) * maxSwarmAmount;
        float wiggleZ = swarmAmount * (Mathf.PerlinNoise((Time.time + randomTimeOffset) * swarmFrequency, 0.8f) - 0.5f) * maxSwarmAmount;

        transform.rotation = Quaternion.Euler(wiggleX, wiggleY, wiggleZ) * transform.rotation;
        transform.position += transform.rotation * Vector3.forward * missileSO.speed * Time.deltaTime;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Missile Collision");
    }
}
