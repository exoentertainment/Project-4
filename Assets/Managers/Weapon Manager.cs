using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] private LayerMask weaponPlatformMask;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] private Transform gamepadMouseCursor;

    #endregion


    
    public void PreviousWeapon()
    {
        Debug.Log("Previous Weapon");
    }
    
    public void NextWeapon()
    {
        Debug.Log("Next Weapon");
    }
    
    //If player clicks on a weapon platform slot, then call the interact function
    public void SelectWeaponPlatform(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray;

            if (playerInput.currentControlScheme == "Keyboard&Mouse")
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            else
            {
                ray = Camera.main.ScreenPointToRay(gamepadMouseCursor.position);
            }

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, weaponPlatformMask))
            {
                if (hit.collider != null)
                {
                    Debug.Log("Selected Platform");
                    // if (hit.collider.gameObject.TryGetComponent<iInteractable>(out iInteractable slot)) 
                    // {
                    //     slot.Interact();
                    // }
                }
            }
        }
    }
}
