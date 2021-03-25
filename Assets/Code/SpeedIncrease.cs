/****************
* Name: Andrew Dahlman-Oeth
* Date:
* Desc:
****************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIncrease : MonoBehaviour
{
    public float dist;
    public float cooldown = 1.0f;
    public float stamp;
    private GameObject player = null;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Get distance
        dist = PlayerSaveData.DistanceRun;

        // If multiple of 25, increase speed
        if((Mathf.Round(dist) % 50f == 0) && (stamp <= Time.time) && (Mathf.Round(dist) != 0))
        {
            Debug.Log(Mathf.Round(dist));
            player.GetComponent<PlayerMovementController>().MoveSpeed += 0.5f;
            // Cooldown timer so we don't increase speed more than once
            stamp = Time.time + cooldown;

        }
        else
        {
            return;
        }
        // Else return
    }
}
