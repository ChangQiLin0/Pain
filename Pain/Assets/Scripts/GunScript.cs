using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    // serializefield allows private objects to be viewed in unity editor
    [SerializeField] private Transform barrel;
    [SerializeField] private float fireRate;
    // lower firerate = faster
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject player;

    public float baseBulletSpread = 10f;
    private float totalSpread;
    public float baseReloadTime = 0.5f;
    private float reloadTimer;
    
    private float fireTimer;
    private float curReloadTime;
    private bool isReloading;
    private int totalAmmo;
    public int maxAmmo;
    public int curAmmo;
    // set to private when not testing


    void Start()
    {
        curAmmo = maxAmmo;
        GetValues();
    }
    
    void Update()
    {
        GetValues();
    }
    void GetValues()
    {
        if (player != null)
        {
            // gets additional bullet spread 
            float addBulletSpread = player.GetComponent<PlayerStats>().bulletSpreadAngle;
            totalSpread = baseBulletSpread + addBulletSpread;

            reloadTimer = player.GetComponent<PlayerStats>().reloadSpeed;

            int ammoBonus = player.GetComponent<PlayerStats>().bonusAmmo;
            totalAmmo = maxAmmo + ammoBonus;
            
        }
    }

    void FixedUpdate()
    {
        DetectShooting();     
        Reloading();
    }

    private void Reloading()
    {
        if (curAmmo <= 0 || (Input.GetKeyDown(KeyCode.R)))
        {
            Debug.Log("RELOADING");
            isReloading = true;
            curAmmo = totalAmmo;
        }

        if (isReloading)
        {
            curReloadTime += Time.fixedDeltaTime;
            if (curReloadTime >= reloadTimer)
            {
                isReloading = false;
                curReloadTime = 0f;
            }
        }
    }


    private void DetectShooting()
    {
        // reads mouse input for left lick (0)
        // change to GetMouseButtonDown for no auto guns
        if (Input.GetMouseButton(0) && (Time.time > fireTimer) && !isReloading)
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        fireTimer = Time.time + fireRate;
        // barrel rotation modifier to spread
        if (totalSpread > 45)
        {
            // max bullet spread is 45
            totalSpread = 45;
        }
        else if (totalSpread < 0)
        {
            // cannot go below 0
            totalSpread = 0;
        }
        // random number generator
        float randomFloat = Random.Range(-totalSpread, totalSpread);
        // creates the set rotation as bulletRotation
        Quaternion bulletRotation = Quaternion.Euler(0f, 0f, randomFloat);
        // creates a bullet at the position of the barrel with applied rotation
        GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation * bulletRotation);
        curAmmo -= 1;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            
    }


}


