using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerWeaponManager : MonoBehaviour
{
    [FormerlySerializedAs("gunListeners")] [SerializeField] private UnityEvent turnGunsOn;
    [SerializeField] private UnityEvent turnGunsOff;
    
    public void FireWeapons(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            turnGunsOn?.Invoke();
        }
        else
        {
            turnGunsOff?.Invoke();
        }
    }
}
