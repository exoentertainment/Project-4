using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region --Serialized Members--
    
    [Header("Movement Settings")]
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float strafeSpeed = 10f;
    [SerializeField] float verticalSpeed = 10f;
    [SerializeField] private float forwardSpeed = 10;
    [SerializeField] float backwardSpeed = 10;
    [SerializeField] float boostSpeed = 10;
    [SerializeField] private float boostRate;
    [SerializeField] float boostDuration;
    [SerializeField] private float rotationLimit;
    
    #endregion

    private string currentControlScheme;

    #region --Enums--

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

    enum VerticalDirection
    {
        Up = 1,
        Down = -1
    }

    #endregion
    
    private int rotateDirection;
    int strafeDirection;
    int verticalDirection;
    
    private bool isRotating;
    private bool isStrafing;
    private bool isVertical;
    private bool isMovingForward;
    bool isMovingBackward;
    private bool isAiming;

    private Vector2 aimAngle;
    float rotationAngle;
    private float lastBoostTime;

    Vector3 currentRotation;
    private float newZ;
    private void Start()
    {
        currentControlScheme = InputManager.Instance.GetCurrentControlScheme();
        
        lastBoostTime = Time.time;
    }

    private void Update()
    {
        //RotateShip();
        StrafeShip();
        MoveVertical();
        MoveHorizontal();
        AimShip();
    }

    #region --Rotation--

    //If player is holding down the rotate keys, rotate ship according to the current direction
    void RotateShip()
    {
        if (isRotating)
        {
            float newZ = transform.eulerAngles.z +(rotationAngle * rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newZ);
        }
    }
    
    //Called by input system, rotates player counter-clockwise
    public void RotateInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRotating = true;
            
            rotationAngle = -context.ReadValue<Vector2>().x;
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
            transform.position += transform.rotation * (Vector3.left * strafeDirection * strafeSpeed * Time.deltaTime);
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

    #region --Vertical Movement

    //If player is holding down the vertical keys, move ship according to the current rotation
    void MoveVertical()
    {
        if (isVertical)
        {
            transform.position += transform.rotation *(Vector3.up * verticalDirection * verticalSpeed * Time.deltaTime);
        }
    }

    //Input calls this when mouse wheel is used
    public void MoveVertical(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isVertical = true;

            if(context.ReadValue<Vector2>().y > 0)
                verticalDirection = (int)VerticalDirection.Up;
            else if(context.ReadValue<Vector2>().y < 0)
                verticalDirection = (int)VerticalDirection.Down;
        }
        else
        {
            isVertical = false;
        }
    }

    //Input calls this when gamepad is used
    public void MoveUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isVertical = true;
            verticalDirection = (int)VerticalDirection.Up;
        }
        else
        {
            isVertical = false;
        }
    }
    
    //Input calls this when gamepad is used
    public void MoveDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isVertical = true;
            verticalDirection = (int)VerticalDirection.Down;
        }
        else
        {
            isVertical = false;
        }
    }
    
    #endregion

    #region --Forward Movement--

    //If player is holding down the accelerate/decelerate keys, move ship according to the current rotation
    void MoveHorizontal()
    {
        if(isMovingForward)
            transform.position += transform.rotation * (transform.forward * forwardSpeed * Time.deltaTime);
        else if(isMovingBackward)
            transform.position += transform.rotation * (Vector3.back * backwardSpeed * Time.deltaTime);
    }

    public void MoveForward(InputAction.CallbackContext context)
    {
        if(context.performed)
            isMovingForward = true;
        else 
            isMovingForward = false;
    }
    
    public void MoveBackward(InputAction.CallbackContext context)
    {
        if(context.performed)
            isMovingBackward = true;
        else 
            isMovingBackward = false;
    }
    
    #endregion

    #region --Aim Ship--

    void AimShip()
    {
        if(isAiming)
        {
            currentRotation.x += (aimAngle.y * rotationSpeed * Time.deltaTime);
            currentRotation.y += (aimAngle.x * rotationSpeed * Time.deltaTime);
            currentRotation.z += (aimAngle.x * -rotationSpeed * Time.deltaTime);
            currentRotation.z = Mathf.Clamp(currentRotation.z, -rotationLimit, rotationLimit);
            
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, currentRotation.z);
        }
    }

    public void AdjustAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            aimAngle = context.ReadValue<Vector2>();
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }
    }
    
    #endregion

    #region --Boost--

    public void Boost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if ((Time.time - lastBoostTime) > boostRate)
            {
                lastBoostTime = Time.time;
                StartCoroutine(BoostRoutine());
            }
        }
    }

    IEnumerator BoostRoutine()
    {
        float boostTime = 0;

        while (boostTime < boostDuration)
        {
            boostTime += Time.deltaTime;
            transform.position += transform.rotation * (Vector3.forward * boostSpeed * Time.deltaTime);
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    #endregion
}
