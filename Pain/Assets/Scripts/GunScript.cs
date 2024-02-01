using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Callbacks;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Transform barrel;
    public GameObject bulletPrefab;
    public GameObject gunObject;
    private GameObject player;

    private float totalBulletSpread; // max 45
    public float baseBulletSpread;
    private float playerBulletSpread;

    public float totalDamage; // total damage (base+playerbonus) * multi, public - accessed by enemy 
    public float baseDamage; // default damage of gun
    private float playerDamage; // any damage bonus/stat increase to player
    private float playerDamageMulti; // same as above. Can be lower than 1 but not 0

    private float totalAmmo; // total ammo player has (base+playerbonus)
    public float baseAmmo; // default ammo of gun
    private float playerAmmo; // bonus ammo by player stat
    public float curAmmo; // current ammo remaining 

    public float baseFireRate; // lower firerate = faster
    private float fireRateTimer; // timer for shooting

    public float baseReloadSpeed; // reload speed of gun itself - no additional bonuses
    private float curReloadTimer; // current cooldown timer for reloading

    private bool isReloading; // boolean condition for if gun is reloading
    public bool isSelected; // should never be true by default unless its the first gun
    public bool isShotgun; // whether or not gun should be treated as a shotgun
    public int lowerBound; // set lower bound used for calcualtions e.g. damage, reload speed and mag size
    public int upperBound; // set upper bound used for calcualtions e.g. damage, reload speed and mag size
    


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        GetValues();
        SpriteLayering();
    }
    private void GetValues()
    {
        playerBulletSpread = player.GetComponent<Player>().bulletSpreadAngle; // get values from playerstat script
        playerDamage = player.GetComponent<Player>().bonusDamage;
        playerDamageMulti = player.GetComponent<Player>().damageMultiplier;

        totalBulletSpread = baseBulletSpread + playerBulletSpread; // calculated total bullet spread
        totalDamage = (baseDamage + playerDamage) * playerDamageMulti; // same but damage

        curAmmo = baseAmmo + player.GetComponent<Player>().bonusAmmo; // set current ammo capacity to player base + gunbase
    }
    private void SpriteLayering()
    {
        if (!isSelected)
        {
            gunObject.GetComponent<SpriteRenderer>().sortingOrder = -3; // subtract 1 from sorting layer so it always shows under the player 
        }
        else
        {
            gunObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
        }
        if (isSelected)
        {
            DetectShooting(); 
            Reloading();
            Aiming();
        }
    }
    private void DetectShooting()
    {
        if (totalBulletSpread > 45)
        {
            totalBulletSpread = 45; // max bullet spread is 45
        }
        else if (totalBulletSpread < 0)
        {
            totalBulletSpread = 0; // cannot go below 0
        }

        if (Input.GetMouseButton(0) && (Time.time > fireRateTimer) && !isReloading && !isShotgun) // reads mouse input for left lick (0) and shotgun is false
        {
            float randomFloat = Random.Range(-totalBulletSpread, totalBulletSpread); // randomise spread in both directions
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, randomFloat); // creates bullet with random spread
            GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation * bulletRotation); // creates bullet object
            bullet.GetComponent<BulletScript>().gunDamage = totalDamage; // fetch component to set value
            
            curAmmo -= 1; // subtract bullet count
            fireRateTimer = Time.time + baseFireRate; // increment time
        }
        else if (Input.GetMouseButton(0) && (Time.time > fireRateTimer) && !isReloading && isShotgun) // is shotgun and leftclick is pressed
        {
            for (int i = 0; i < curAmmo; i++) // allows number of bullets shot to be scaled with mag size
            {
                float randomFloat = Random.Range(-totalBulletSpread, totalBulletSpread); // randomise spread in both directions
                Quaternion bulletRotation = Quaternion.Euler(0f, 0f, randomFloat); // creates bullet with random spread
                GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation * bulletRotation); // creates bullet object
                bullet.GetComponent<BulletScript>().gunDamage = totalDamage; // fetch component to set value
            }
            curAmmo = 0; // after every shot, ammo is set to 0
        }
    }

    private void Reloading()
    {
        if (curAmmo <= 0 || Input.GetKeyDown(KeyCode.R)) // check if reload conditions are met
        {
            Debug.Log("Reloading");
            isReloading = true; // set reload to true so player cant shoot
            curAmmo = totalAmmo; // set ammo to max
        }

        if (isReloading)
        {
            if (curReloadTimer >= baseReloadSpeed) // if time passed is greater than reload time
            {
                isReloading = false; // no longer reloading
                curReloadTimer = 0f; // reset timer
            }
            curReloadTimer += Time.fixedDeltaTime; // add time 
        }
    }

    private void Aiming()
    {
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position); // find direction of mouse 
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // calculates angle and converts to degrees using mathf.rad2deg
        transform.eulerAngles = new Vector3(0, 0, angle); // apply rotation

        Vector3 localScale = Vector3.one;

        if (angle > 89 || angle < -89) // 89 prevents collision/interference with flip/over the top flipping
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }
        transform.localScale = localScale; // finalises the flip
    }
}
