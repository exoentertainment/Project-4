using System;
using UnityEngine;
using System.Collections;

public class PDCProjectile : MonoBehaviour
{
    [SerializeField] ProjectileSO projectileSO;

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
        transform.position += transform.rotation * Vector3.forward * projectileSO.speed * Time.deltaTime;
    }

    IEnumerator DeactivateRoutine()
    {
        yield return new WaitForSeconds(projectileSO.duration);
        
        isActivated = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    { 
        if(projectileSO.impactPrefab != null)
            Instantiate(projectileSO.impactPrefab, other.contacts[0].point, Quaternion.identity);
        
        gameObject.SetActive(false);
    }
}
