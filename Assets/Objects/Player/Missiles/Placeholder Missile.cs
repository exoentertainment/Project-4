using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderMissile : MonoBehaviour
{
    [SerializeField] MissileSO missileSO;

    private GameObject target;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, missileSO.duration);
        AssignTargetIcon();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += transform.rotation * Vector3.forward * missileSO.speed * Time.deltaTime;
    }

    void AssignTargetIcon()
    {
        GameObject targetIcon = Instantiate(missileSO.targetIcon, transform.position, transform.rotation);
    }

    IEnumerator AssignTargetIconRoutine(GameObject targetIcon)
    {
        yield return new WaitForSeconds(missileSO.duration);
        
        Destroy(targetIcon);
    }
    
    void SearchForTarget()
    {
        
    }
}
