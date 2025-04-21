using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BaseTurretHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private TurretSO turretSO;
    [SerializeField] UnityEvent onDeath;

    BoxCollider boxCollider;
    private float currentHealth;

    private bool isDead;
    private bool isHit;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        currentHealth = turretSO.maxHealth;
    }

    private void Update()
    {
        isHit = false;
    }

    public void TakeDamage(float damage)
    {
        if (!isHit)
        {
            isHit = true;
            currentHealth -= damage;
        }
        
        if(currentHealth <= 0 && !isDead)
            OnDeath();
    }

    void OnDeath()
    {
        isDead = true;
        
        //Invoke onDeath event
        onDeath?.Invoke();

        if (turretSO.explosionPrefab != null)
            StartCoroutine(SpawnExplosionsRoutine());
    }
    
    IEnumerator SpawnExplosionsRoutine()
    {
        float startTime = Time.time;

        do
        {
            Vector3 explosionPos = new Vector3(Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
                Random.Range(boxCollider.bounds.min.y, boxCollider.bounds.max.y),
                Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z));
            Instantiate(turretSO.explosionPrefab, explosionPos, Quaternion.identity);

            yield return new WaitForSeconds(turretSO.explosionFrequency);
        } 
        while ((Time.time - startTime) < turretSO.explosionDuration);
        
        //call reactor death at end of routine
        
        Destroy(gameObject);
    }
}
