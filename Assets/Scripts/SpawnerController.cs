using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    // [SerializeField] private Transform[] spawnerLocations;
    // [SerializeField] private GameObject[] prefabsToSpawn;
    // [SerializeField] private GameObject[] prefabsToClone;
    
    [SerializeField] private bool playerDetected;

    private void Awake()
    {
    }
    
    private void Start()
    {
        //prefabsToClone = new GameObject[prefabsToSpawn.Length];
        //Spawn();
    }

    /*private void Spawn()
    {
        for (var i = 0; i < prefabsToSpawn.Length; i++)
        {
            prefabsToClone[i] = Instantiate(prefabsToSpawn[i], spawnerLocations[i].transform.position,
                Quaternion.Euler(0, 0, 0) )as GameObject;
        }
    }

    private void DestroyClonedGameObjects()
    {
        // Destroy all cloned game objects
        for (var i = 0; i < prefabsToClone.Length; i++)
        {
            Destroy(prefabsToClone[i]);
        }
    }

    private void Respawn()
    {
        // First destroy all already cloned game objects
        DestroyClonedGameObjects();
        
        // Spawn all game objects
        Spawn();
    }*/

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Doggy");
        if (other.tag == "Player")
        {
            //playerDetected = true;
            Debug.Log("wolfy");
        }
    }

    private void OnTriggerExit(Collider other) //DangerZone detection
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }
    
}
