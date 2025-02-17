using UnityEngine;

public class PlaceholderMissileLauncher : MonoBehaviour
{
    #region --Serialized Fields--

    [SerializeField] MissileLauncherSO missileLauncherSO;

    #endregion
    
    float lastFireTime;
    private bool isFiring;
    
    private void Start()
    {
        lastFireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }
    
    public void ActivateGun()
    {
        isFiring = true;
    }

    public void DeactivateGun()
    {
        isFiring = false;
    }
    
    void Fire()
    {
        if(isFiring)
            if ((Time.time - lastFireTime) > missileLauncherSO.fireRate)
            {
                lastFireTime = Time.time;
                
                Instantiate(missileLauncherSO.missileSO.missilePrefab, transform.position, transform.rotation);
            }
    }
}
