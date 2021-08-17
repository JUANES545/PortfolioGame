using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private Transform spawnerLocations;
    [SerializeField] private GameObject[] prefabsToSpawn;
    [SerializeField] private GameObject[] prefabsToClone;
    
    [SerializeField] private bool playerDetected;

    private void Awake()
    {
    }
    
    private void Start()
    {
        //prefabsToClone = new GameObject[prefabsToSpawn.Length];
        //prefabsToClone = FindObjectOfType<SpawnerController>()
        //Spawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            ReSpawnPrefab();
    }

    private void Spawn()
    {
        for (var i = 0; i < prefabsToSpawn.Length; i++)
        {
            prefabsToClone[i] = Instantiate(prefabsToSpawn[i], spawnerLocations.transform.position,
                Quaternion.Euler(0, 0, 0) )as GameObject;
        }
    }

    private void DestroyClonedGameObjects()
    {
        // Destroy all cloned game objects
        foreach (var t in prefabsToClone)
        {
            Destroy(t);
        }
    }

    private void Respawn()
    {
        // First destroy all already cloned game objects
        DestroyClonedGameObjects();
        
        // Spawn all game objects
        Spawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }

    private void ReSpawnPrefab()
    {
        if (playerDetected)
        {
            Respawn();
        }
    }
    
}
