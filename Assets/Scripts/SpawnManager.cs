using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Spawnar zombieprefabsen i olika positioner
    public GameObject zombiePrefabs;
    private float spawnRangeX = 180;
    private float spawnPosZ = 10;
    private float startDelay = 2;
    private float repeatInterval = 2;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnZombies", startDelay, repeatInterval);
    }

    void SpawnZombies()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
        Instantiate(zombiePrefabs, spawnPos, zombiePrefabs.transform.rotation);
    }
}
