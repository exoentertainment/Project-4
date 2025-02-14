using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region --Serialized Members--
    
    [SerializeField] float rotationSpeed = 10f;
    
    #endregion

    private string currentControlScheme;

    private bool isRotating;
    
    private void Start()
    {
        currentControlScheme = InputManager.Instance.GetCurrentActionMap();
    }

    public void RotatePlayerLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isRotating = true;
            
            if (InputManager.Instance.GetCurrentActionMap() == "Keyboard&Mouse")
            {
                Debug.Log("Keyboard Left");
            }
            else if (InputManager.Instance.GetCurrentActionMap() == "Gamepad")
            {
                Debug.Log("Gamepad left");
            }
        }
        else
        {
            isRotating = false;
        }
    }
}
