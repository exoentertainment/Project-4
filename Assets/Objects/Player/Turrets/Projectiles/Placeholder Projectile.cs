using System;
using UnityEngine;

public class PlaceholderProjectile : MonoBehaviour
{
    [SerializeField] ProjectileSO projectileSO;

    //
    //private bool isActivated;
    private void Start()
    {
        Destroy(gameObject, projectileSO.duration);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += transform.rotation * Vector3.forward * projectileSO.speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        Instantiate(projectileSO.impactPrefab, other.contacts[0].point, Quaternion.identity);
        Destroy(gameObject);
    }
}
