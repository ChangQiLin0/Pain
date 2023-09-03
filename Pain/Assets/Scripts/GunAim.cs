using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAim : MonoBehaviour
{
    [SerializeField] private Transform player;

    void Update()
    {
        HandleAiming();
    }

    void HandleAiming()
    {
        // rotation using cursor
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
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
