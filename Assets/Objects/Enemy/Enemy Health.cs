using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    #region --Serialized Fields--

    [SerializeField] private EnemySO enemySO;
    [SerializeField] private UnityEvent onDeath;

    #endregion
    
    float currentHealth;
    bool isDead;
    MeshCollider meshCollider;

    private bool isHit;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    private void Start()
    {
        currentHealth = enemySO.maxHealth;
    }

    private void Update()
    {
        isHit = false;
    }

    public void TakeDamage(float damage)
    {
        if (!isHit)
        {
            currentHealth -= damage;
            isHit = true;
        }

        if(currentHealth <= 0 && !isDead)
            OnDeath();
    }

    void OnDeath()
    {
        isDead = true;
        
        //Invoke onDeath event
        onDeath?.Invoke();
        
        //start coroutine that spawns explosions along ship
        StartCoroutine(SpawnExplosionsRoutine());
        
        if(enemySO.reactorExplosionPrefab != null)
            StartCoroutine(SpawnReactorExplosion());
    }

    IEnumerator SpawnExplosionsRoutine()
    {
        float startTime = Time.time;

        do
        {
            Vector3 explosionPos = new Vector3(Random.Range(meshCollider.bounds.min.x, meshCollider.bounds.max.x),
                Random.Range(meshCollider.bounds.min.y, meshCollider.bounds.max.y),
                Random.Range(meshCollider.bounds.min.z, meshCollider.bounds.max.z));
            Instantiate(enemySO.explosionPrefab, explosionPos, Quaternion.identity);

            yield return new WaitForSeconds(enemySO.explosionFrequency);
        } 
        while ((Time.time - startTime) < enemySO.explosionDuration);
        
        //call reactor death at end of routine
        
        Destroy(gameObject);
    }

    IEnumerator SpawnReactorExplosion()
    {
        yield return new WaitForSeconds(enemySO.explosionDuration * .75f);
        
        Instantiate(enemySO.reactorExplosionPrefab, transform.position, Quaternion.identity);
    }
}
