using UnityEngine;
using VSX.UniversalVehicleCombat;

public class MissileLockIconRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        RotateIcon();
    }

    void RotateIcon()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }
}
