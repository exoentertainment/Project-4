using System;
using UnityEngine;
using System.Collections;

public class PDCProjectile : MonoBehaviour
{
    [SerializeField] BaseProjectileSO projectileSO;

    private bool isActivated;

    // Update is called once per frame
    void Update()
    {
        Move();

        if (!isActivated)
        {
            isActivated = true;
            StartCoroutine(DeactivateRoutine());
        }
    }

    void Move()
    {
        transform.position += transform.forward * (projectileSO.speed * Time.deltaTime);
    }

    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(projectileSO.duration);
        
        isActivated = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    { 
        Debug.Log(other.gameObject.name);
        if(projectileSO.impactPrefab != null)
            Instantiate(projectileSO.impactPrefab, other.GetContact(0).point, Quaternion.identity);
        
        other.gameObject.GetComponent<IDamageable>()?.TakeDamage(projectileSO.damage);
        
        gameObject.SetActive(false);
    }
}
