using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    void Update() 
    {   
        // bullet moves based on the speed and will be the same on all fps
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
    }
}
