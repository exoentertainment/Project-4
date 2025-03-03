using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    #region --Serialized Fields--

    [SerializeField] private EnemyHealthSO healthSO;
    [SerializeField] private UnityEvent onDeath;

    #endregion
    
    float currentHealth;
    bool isDead;
    MeshCollider meshCollider;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    private void Start()
    {
        currentHealth = healthSO.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
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
    }

    IEnumerator SpawnExplosionsRoutine()
    {
        float startTime = Time.time;

        while ((Time.time - startTime) < healthSO.explosionDuration)
        {
            Vector3 explosionPos = new Vector3(Random.Range(meshCollider.bounds.min.x, meshCollider.bounds.max.x), Random.Range(meshCollider.bounds.min.y, meshCollider.bounds.max.y), Random.Range(meshCollider.bounds.min.z, meshCollider.bounds.max.z));
            Instantiate(healthSO.explosionPrefab, explosionPos, Quaternion.identity);
            
            yield return new WaitForSeconds(healthSO.explosionFrequency);
        }
        
        //call reactor death at end of routine
    }

    void SpawnReactorExplosion()
    {
        
    }
}
