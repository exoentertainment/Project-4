using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraManager : MonoBehaviour
{
    #region --Serialized Fields--

    [FormerlySerializedAs("phaseOneCameras")] [FormerlySerializedAs("planetCamera")] [SerializeField] GameObject phaseOneCamera;
    [SerializeField] GameObject playerCamera;
    [SerializeField] private Transform planet;

    [SerializeField] private float planetRotateSpeed;
    
    #endregion

    public static CameraManager Instance;
    
    enum RotationDirection
    {
        rotateLeft = -1,
        rotateRight = 1
    }
    
    bool isRotating;
    int rotateDirection = (int)RotationDirection.rotateLeft;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Update()
    {
        RotatePlanet();
    }

    //Disable all cameras associated with Phase 1 and enable the player ship camera
    public void InitPhaseTwoCameras()
    {
        phaseOneCamera.SetActive(false);
        
        playerCamera.SetActive(true);
    }

    public void InitPhaseOneCameras()
    {
        playerCamera.SetActive(false);
        phaseOneCamera.SetActive(true);
    }
    
    void RotatePlanet()
    {
        if (isRotating)
        {
            float newAngle = phaseOneCamera.transform.eulerAngles.y + (planetRotateSpeed * Time.unscaledDeltaTime * rotateDirection);
        
            phaseOneCamera.transform.rotation = Quaternion.Euler(phaseOneCamera.transform.rotation.eulerAngles.x, newAngle, phaseOneCamera.transform.rotation.eulerAngles.z);
        }
    }

    public bool ObjectInCameraView(Transform objectInCameraView)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(objectInCameraView.position);

        return (viewPos.x >= 0 && viewPos.y >= 0) && (viewPos.x <= 1 && viewPos.y <= 1) && viewPos.z >= 0;
    }
    
    #region --Input Listeners

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

    #endregion

}
