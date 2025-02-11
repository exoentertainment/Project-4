using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] private LayerMask weaponPlatformMask;
    [SerializeField] private LayerMask weaponPlatformUIMask;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] private Transform gamepadMouseCursor;

    [SerializeField] private WeaponPlatformSO[] weaponPlatforms;
    [SerializeField] private Transform weaponPreviewPane;
    [SerializeField] private int weaponPreviewRotateSpeed;

    [SerializeField] TMP_Text uiDamageText;
    [SerializeField] TMP_Text uiRangeText;
    [SerializeField] TMP_Text uiCostText;
    [SerializeField] TMP_Text uiRoFText;
    [SerializeField] TMP_Text uiDescriptionText;
    [SerializeField] private TMP_Text uiCurrentResourcesText;

    [SerializeField] private int currentResources;
    
    #endregion

    private GameObject weaponPlatformPreview;
    private int currentWeaponPlatform;
    
    private void Start()
    {
        uiCurrentResourcesText.text = "Resources: " + currentResources.ToString();
        
        PopulatePreviewPanel();
    }

    private void Update()
    {
        RotateWeaponPlatformPreview();
    }

    //Instantiates the currently selected weapon and spawns it at the center of world for the preview camera to capture
    void PopulatePreviewPanel()
    {
        
        //Instantiate weapon and fit to preview window
        weaponPlatformPreview = Instantiate(weaponPlatforms[currentWeaponPlatform].weaponPrefab, Vector3.zero, Quaternion.identity);
        weaponPlatformPreview.layer = LayerMask.NameToLayer("UI Preview");
        weaponPlatformPreview.transform.localScale = new Vector3(weaponPlatforms[currentWeaponPlatform].UIScale, weaponPlatforms[currentWeaponPlatform].UIScale,
            weaponPlatforms[currentWeaponPlatform].UIScale);
        
        //Populate weapon stat windows
        uiDamageText.text = "Damage: " + weaponPlatforms[currentWeaponPlatform].damage.ToString();
        uiRangeText.text = "Range: " + weaponPlatforms[currentWeaponPlatform].range.ToString();
        uiCostText.text = "Cost: " + weaponPlatforms[currentWeaponPlatform].cost.ToString();
        uiRoFText.text = "RoF: " + weaponPlatforms[currentWeaponPlatform].fireRate.ToString();
        uiDescriptionText.text = weaponPlatforms[currentWeaponPlatform].weaponDescription.ToString();

    }

    void RotateWeaponPlatformPreview()
    {
        weaponPlatformPreview.transform.Rotate(Vector3.up, weaponPreviewRotateSpeed * Time.deltaTime);
    }
    
    //Move to the previous weapon in the array then populate the preview window
    public void PreviousWeapon()
    {
        if (currentWeaponPlatform - 1 >= 0)
            currentWeaponPlatform--;
        else
            currentWeaponPlatform = weaponPlatforms.Length - 1;
        
        if(weaponPlatformPreview != null)
            Destroy(weaponPlatformPreview);
        
        PopulatePreviewPanel();
    }
    
    //Move to the next weapon in the array then populate the preview window
    public void NextWeapon()
    {
        if (currentWeaponPlatform + 1 < weaponPlatforms.Length)
            currentWeaponPlatform++;
        else
            currentWeaponPlatform = 0;
        
        if(weaponPlatformPreview != null)
            Destroy(weaponPlatformPreview);
        
        PopulatePreviewPanel();
    }
    
    //If player clicks on a weapon platform slot, then call the interact function
    public void SelectWeaponPlatform(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray;

            //If player is using the keyboard/mouse then shoot a ray from the mouse's position
            if (playerInput.currentControlScheme == "Keyboard&Mouse")
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
            //If player is using the gamepad then shoot a ray from the virtual mouse cursor
            else
            {
                ray = Camera.main.ScreenPointToRay(gamepadMouseCursor.position);
            }

            //If the raycast hits an object under the weapon platform mask then check for resources and place the selected weapon
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, weaponPlatformMask))
            {
                if (hit.collider != null)
                {
                    if (currentResources - weaponPlatforms[currentWeaponPlatform].cost >= 0)
                    {
                        hit.collider.GetComponent<WeaponPlatformSpot>()
                            .PlaceWeapon(weaponPlatforms[currentWeaponPlatform].weaponPrefab);
                        
                        currentResources -= weaponPlatforms[currentWeaponPlatform].cost;
                        
                        uiCurrentResourcesText.text = "Resources: " + currentResources.ToString();
                    }
                    else
                    {
                        //Play sound
                    }
                }
            }
        }
    }
}
