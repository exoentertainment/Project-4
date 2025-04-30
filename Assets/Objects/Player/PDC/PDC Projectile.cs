using System;
using UnityEngine;
using System.Collections;

public class PDCProjectile : MonoBehaviour
{
    [SerializeField] BaseProjectileSO projectileSO;
    
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += transform.forward * (projectileSO.speed * Time.deltaTime);
    }

    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(projectileSO.duration);
        
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    { 
        if(projectileSO.impactPrefab != null)
            Instantiate(projectileSO.impactPrefab, other.GetContact(0).point, Quaternion.identity);
        
        other.gameObject.GetComponent<IDamageable>()?.TakeDamage(projectileSO.damage);
        
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateRoutine());
    }
}
