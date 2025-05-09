using UnityEngine;

public class RotateTurretObject : MonoBehaviour
{
    [SerializeField] private Transform rotatingObject;
    [SerializeField] protected TurretSO platformSO;
    [SerializeField] private float minRotation;
    [SerializeField] float maxRotation;
    
    float currentAngle;
    GameObject target;
    
    protected void Update()
    {
        if (target != null)
        {
            if (target.activeSelf)
            {
                //Debug.DrawLine(raycastOrigin.position, raycastOrigin.position + (raycastOrigin.transform.forward * platformSO.projectileSO.range), Color.red);
                //Debug.DrawRay(raycastOrigin.position, raycastOrigin.position + (transform.forward * platformSO.projectileSO.range), Color.red);
                RotateTowardsTarget();
            }
        }
    }
    
    void RotateTowardsTarget()
    {
        Vector3 dir = transform.InverseTransformDirection(target.transform.position - transform.position);
        dir.y = 0;
        
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        targetAngle = Mathf.Clamp(targetAngle, minRotation, maxRotation);
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, platformSO.baseTrackingSpeed * Time.deltaTime);
        Debug.Log(gameObject.name + " " + currentAngle);
        rotatingObject.localEulerAngles = Vector3.up * currentAngle; 
    }
    
    public void SetTarget(GameObject target)
    {
        this.target = target;    
    }
}
