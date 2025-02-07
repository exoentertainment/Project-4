using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] CinemachineCamera planetCamera;
    [SerializeField] private Transform planet;

    [SerializeField] private float planetRotateSpeed;
    
    #endregion

    enum RotationDirection
    {
        rotateLeft = -1,
        rotateRight = 1
    }
    
    bool isRotating;
    int rotateDirection = (int)RotationDirection.rotateLeft;

    private void Update()
    {
        RotatePlanet();
    }

    void RotatePlanet()
    {
        if (isRotating)
        {
            float newAngle = planet.eulerAngles.y + (planetRotateSpeed * Time.deltaTime * rotateDirection);
        
            planet.rotation = Quaternion.Euler(planet.rotation.eulerAngles.x, newAngle, planet.rotation.eulerAngles.z);
        }
    }
    
    public void RotateCameraLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRotating = true;
            rotateDirection = (int)RotationDirection.rotateLeft;
        }
        else if (context.canceled)
        {
            isRotating = false;
        }
    }
    
    public void RotateCameraRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRotating = true;
            rotateDirection = (int)RotationDirection.rotateRight;
        }
        else if (context.canceled)
        {
            isRotating = false;
        }
    }
}
