using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour
{
    private Transform target;
    public float offset = 0;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        EnemyAiming();
    }

    void EnemyAiming()
    {
        // similar code from GunAim.cs
        // get direction of player 
        Vector2 dir = target.position - transform.position;
        // calculates angle and converts to degrees using mathf.rad2deg
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
 
        // flip gun when past y axis

        Vector3 localScale = Vector3.one;

        if (angle > 90 || angle < -90)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }
        // flips based on localScale
        transform.localScale = localScale;

    }
}
