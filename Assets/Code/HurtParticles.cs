/****************
* Name: Andrew Dahlman-Oeth
* Date: 3/31/21
* Desc: Emits particles on collision with obstacle
****************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtParticles : MonoBehaviour
{
    public static bool hurtPlr = false;
    public GameObject plr;
    public GameObject particles;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (hurtPlr) 
        {
            var ps = GetComponent<ParticleSystem>();
            ps.Emit(6);
            hurtPlr = false;
         }
    }
}
