using UnityEngine;
using System.Collections;

public class MissileLauncherTurret : BaseTurret
{
    void Awake()
    {
        base.Awake();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    
    protected override IEnumerator FireRoutine()
    {
        lastTimeOnTarget = Time.time;
        lastFireTime = Time.time;

        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject projectile = projectilePool.GetPooledObject(); 
            if (projectile != null) 
            {
                projectile.transform.position = spawnPoint.position;
                projectile.transform.rotation = transform.rotation;
                projectile.SetActive(true);
                projectile.GetComponent<PlaceholderMissile>().SetTarget(target);
            }
            
            // GameObject missile = Instantiate(platformSO.projectileSO.projectilePrefab, spawnPoint.position, transform.rotation);
            // missile.GetComponent<PlaceholderMissile>().SetTarget(target);

            if (platformSO.projectileSO.dischargePrefab != null)
                Instantiate(platformSO.projectileSO.dischargePrefab, spawnPoint.position,
                    platformTurret.transform.rotation);
            
            yield return new WaitForSeconds(platformSO.barrelFireDelay);
        }
        
        if(AudioManager.instance != null)
            AudioManager.instance.PlaySound(platformSO.fireSFX);
    }
}
