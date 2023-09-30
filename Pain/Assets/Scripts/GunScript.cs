using System.Collections;
using System.Collections.Generic;
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
    private float playerDamageMulti; // same as ^. Can be lower than 1 but not 0

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


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAmmo = player.GetComponent<Player>().bonusAmmo; // needs to be called to set initial max ammo
        totalAmmo = baseAmmo + playerAmmo;
        curAmmo = totalAmmo;
    }

    void FixedUpdate()
    {
        GetValues();
        if (gameObject.CompareTag("Loot"))
        {
            gunObject.GetComponent<SpriteRenderer>().sortingOrder = 4; // subtract 1 from sorting layer so it always shows under the player 
        }
        if (isSelected)
        {
            DetectShooting(); 
            Reloading();
            Aiming();
        }
    }
    void GetValues()
    {
        playerBulletSpread = player.GetComponent<Player>().bulletSpreadAngle; // get values from playerstat script
        playerDamage = player.GetComponent<Player>().bonusDamage;
        playerDamageMulti = player.GetComponent<Player>().damageMultiplier;

        totalBulletSpread = baseBulletSpread + playerBulletSpread; // calculated total bullet spread
        totalDamage = (baseDamage + playerDamage) * playerDamageMulti; // same but damage
    }

    void DetectShooting()
    {
        if (Input.GetMouseButton(0) && (Time.time > fireRateTimer) && !isReloading) // reads mouse input for left lick (0)
        {
            if (totalBulletSpread > 45)
            {
                totalBulletSpread = 45; // max bullet spread is 45
            }
            else if (totalBulletSpread < 0)
            {
                totalBulletSpread = 0; // cannot go below 0
            }
            // random number generator
            float randomFloat = Random.Range(-totalBulletSpread, totalBulletSpread); // randomise spread in both directions
            // creates the set rotation as bulletRotation
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, randomFloat); // creates bullet with random spread
            // creates a bullet at the position of the barrel with applied rotation
            GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation * bulletRotation); // creates bullet object
            bullet.GetComponent<BulletScript>().gunDamage = totalDamage; // fetch component to set value
            
            curAmmo -= 1; // subtract bullet count
            fireRateTimer = Time.time + baseFireRate; // increment time
        }
    }

    void Reloading()
    {
        if (curAmmo <= 0 || Input.GetKeyDown(KeyCode.R)) // check if reload conditions are met
        {
            
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

    void Aiming()
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
