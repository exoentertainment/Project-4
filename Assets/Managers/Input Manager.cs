using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region --Serialized Members--
    
    [SerializeField] PlayerInput playerInput;
    [SerializeField] private GameObject virtualMouse;
    
    #endregion
    
    private GameObject pauseMenu;
    public static InputManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenSettings(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Time.timeScale = 0;

            if (pauseMenu == null)
            {
                pauseMenu = (GameObject)Instantiate(Resources.Load("Pause Menu"));
                
                if(playerInput.currentActionMap.name == "Gamepad")
                    virtualMouse.SetActive(true);
                else
                    Cursor.visible = true;
            }
        }
    }
    
    //Switch player action map to Player
    public void SwitchInputProfilePhaseTwo()
    {
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void SwitchInputProfilePhaseOne()
    {
        playerInput.SwitchCurrentActionMap("UI");
    }
    
    public void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Mouse.current.WarpCursorPosition(Vector2.zero);
    }

    public void ShowMouse()
    {
        if (playerInput.currentActionMap.name == "Keyboard&Mouse")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (playerInput.currentActionMap.name == "Gamepad")
            virtualMouse.SetActive(true);
    }
    
    public string GetCurrentControlScheme()
    {
        return playerInput.currentControlScheme;
    }

    public string GetCurrentActionMap() => playerInput.currentActionMap.name;
}
