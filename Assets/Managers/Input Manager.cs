using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public void OpenSettings(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Open Settings");
        }
    }
    
}
