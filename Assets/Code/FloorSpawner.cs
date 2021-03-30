//------------------------------------------------------------------------------
//
// File Name:	FloorSpawner.cs
// Desc:        Spawns objects randomly on the floor
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
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

public class FloorSpawner : MonoBehaviour
{
    // Object spawn timing
    public Vector2 SpawnInterval = new Vector2(0, 1);
    // Whether to align object spawn positions to grid
    public bool AlignSpawnPosition = false;

    // Object prefabs and spawn chances
    public GameObject Object1Prefab = null;
    public int Object1SpawnChance = 1;
    public GameObject Object2Prefab = null;
    public int Object2SpawnChance = 1;


    // Private variables
    private int totalChance = 0;
    private float timeTilNextSpawn = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        totalChance = Object1SpawnChance + Object2SpawnChance;
    }

    // Update is called once per frame
    void Update()
    {
        timeTilNextSpawn -= Time.deltaTime;

        if(timeTilNextSpawn <= 0.0f)
        {
            timeTilNextSpawn += Random.Range(SpawnInterval.x, SpawnInterval.y);
            SpawnRandomObject();
        }
    }

    void SpawnRandomObject()
    {
        // Roll die to determine which object to spawn
        int dieRoll = Random.Range(1, totalChance + 1);

        // Determine spawn position
        Vector3 spawnPosition = transform.position 
            + new Vector3(0, 0, 0);

        GameObject spawnedObject = null;

        // Align to grid
        if(AlignSpawnPosition)
        {
            spawnPosition.x = Mathf.Floor(spawnPosition.x);
            spawnPosition.y = Mathf.Floor(spawnPosition.y) + 0.5f;
        }

        // Create object based on die roll
        if(dieRoll <= Object1SpawnChance)
        {
            spawnedObject = Instantiate(Object1Prefab);
        }
        else if (dieRoll <= totalChance)
        {
            spawnedObject = Instantiate(Object2Prefab);
        }

        // Set object position
        spawnedObject.transform.position = spawnPosition;
    }
}
