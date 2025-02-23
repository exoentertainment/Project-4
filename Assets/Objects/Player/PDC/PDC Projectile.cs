using UnityEngine;
using System.Collections;

public class PDCProjectile : MonoBehaviour
{
    [SerializeField] ProjectileSO projectileSO;

    private bool isActivated;
    private void Start()
    {
        //Destroy(gameObject, projectileSO.duration);
        
    }

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
}
