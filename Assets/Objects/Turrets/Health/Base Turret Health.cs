using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseTurretHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private TurretSO turretSO;
    [SerializeField] UnityEvent onDeath;

    private float currentHealth;

    private bool isDead;
    private bool isHit;

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
            Debug.Log("hit");
            isHit = true;
            //currentHealth -= damage;
        }
        
        if(currentHealth <= 0 && !isDead)
            OnDeath();
    }

    void OnDeath()
    {
        isDead = true;
        
        //Invoke onDeath event
        onDeath?.Invoke();
    }
}
