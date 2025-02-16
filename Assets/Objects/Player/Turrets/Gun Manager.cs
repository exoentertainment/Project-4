using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GunManager : MonoBehaviour
{
    [FormerlySerializedAs("gunListeners")] [SerializeField] private UnityEvent turnGunsOn;
    [SerializeField] private UnityEvent turnGunsOff;
    
    public void FireGuns(InputAction.CallbackContext context)
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
