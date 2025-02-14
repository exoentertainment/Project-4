using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region --Serialized Members--
    
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float strafeSpeed = 10f;
    
    #endregion

    private string currentControlScheme;

    enum RotateDirection
    {
        Left = 1,
        Right = -1
    }
    
    enum StrafeDirection
    {
        Left = 1,
        Right = -1
    }

    private int rotateDirection;
    int strafeDirection;
    
    private bool isRotating;
    private bool isStrafing;
    
    private void Start()
    {
        currentControlScheme = InputManager.Instance.GetCurrentActionMap();
    }

    private void Update()
    {
        RotateShip();
        StrafeShip();
    }

    #region --Rotation--

    //If player is holding down the rotate keys, rotate ship according to the current direction
    void RotateShip()
    {
        if (isRotating)
        {
            float newZ = transform.eulerAngles.z +(rotateDirection * rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newZ);
        }
    }
    
    //Called by input system, rotates player counter-clockwise
    public void RotatePlayerLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRotating = true;
            
            rotateDirection = (int)RotateDirection.Left;
        }
        else
        {
            isRotating = false;
        }
    }
    
    //Called by input system, rotates player clockwise
    public void RotatePlayerRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRotating = true;

            rotateDirection = (int)RotateDirection.Right;
        }
        else
        {
            isRotating = false;
        }
    }

    #endregion

    #region --Strafing--

    //If player is holding down the strafe keys, strafe ship according to the current rotation
    void StrafeShip()
    {
        if (isStrafing)
        {
            transform.position += transform.rotation *(Vector3.left * strafeDirection * strafeSpeed * Time.deltaTime);
        }
    }
    
    //Called by input system, strafes player left
    public void StrafeLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isStrafing = true;

            strafeDirection = (int)StrafeDirection.Left;
        }
        else
        {
            isStrafing = false;
        }
    }

    //Called by input system, strafes player right
    public void StrafeRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isStrafing = true;

            strafeDirection = (int)StrafeDirection.Right;
        }
        else
        {
            isStrafing = false;
        }
    }
    
    #endregion

}
