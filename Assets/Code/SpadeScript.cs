//------------------------------------------------------------------------------
//
// File Name:	SpadeScript.cs
// Desc:        Allows player to shoot projectile that destroys obstacles
// Author(s):	Sim McQueen
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeScript : MonoBehaviour
{
    public int SpadeAmmo = 0;
    public Transform firePoint;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(SpadeAmmo > 0)
            {
                Shoot();
                SpadeAmmo -= 1;
            }
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
