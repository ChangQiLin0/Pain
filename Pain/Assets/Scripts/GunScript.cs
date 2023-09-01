using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    // serializefield allows private objects to be viewed in unity editor
    [SerializeField] private Transform barrel;
    [SerializeField] private float fireRate;
    // lower firerate = faster
    [SerializeField] private GameObject bullet;

    private float fireTimer;


    void Update()
    {
        DetectShooting();
    }

    private void DetectShooting()
    {
        // reads mouse input for left lick (0)
        // change to GetMouseButtonDown for no auto guns
        if (Input.GetMouseButton(0) && CanShoot())
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        fireTimer = Time.time + fireRate;
        // creates a bullet at the position of the barrel 
        Instantiate(bullet, barrel.position, barrel.rotation);
            
    }

    private bool CanShoot()
    {
        // checks if shooting timer is up
        return Time.time > fireTimer;
    }

}


