//------------------------------------------------------------------------------
//
// File Name:	CardAnimator.cs
// Author(s):	Sim McQueen
//              Andrew Dahlman-Oeth
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingCardScript : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }
    
    void FixedUpdate()
    {
	    gameObject.transform.Rotate(0, 0, +5); //Rotate(x, y, z) + = add one per update
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        if(hitInfo.gameObject.CompareTag("Destructible"))
        {
            Destroy(hitInfo.gameObject);
            Destroy(gameObject);
        }
        else if(hitInfo.gameObject.CompareTag("Obstacle")) 
        {
            Destroy(hitInfo.gameObject);
            Destroy(gameObject);
        }
    }


}
