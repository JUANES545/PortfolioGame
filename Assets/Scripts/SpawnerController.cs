using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] private Transform spawnerLocations;
    [SerializeField] private GameObject prefabsToSpawn;
    [SerializeField] private GameObject prefabsToClone;
    
    [SerializeField] private bool playerDetected;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            ReSpawnPrefab();
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
        if (!playerDetected) return;
        Destroy(prefabsToClone, 0.001f);
        prefabsToClone = Instantiate(prefabsToSpawn, spawnerLocations.transform.position,
            Quaternion.Euler(0, 0, 0));
    }
    
}
