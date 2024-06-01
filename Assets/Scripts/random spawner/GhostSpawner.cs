using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject[] ghostPrefabs; // Array of ghost prefabs with different skins
    public int numberOfGhosts = 10; // Number of ghosts to spawn
    public Vector3 spawnAreaSize = new Vector3(50, 0, 50); // Size of the area within which to spawn ghosts

    void Start()
    {
        SpawnGhosts();
    }

    void SpawnGhosts()
    {
        for (int i = 0; i < numberOfGhosts; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject randomGhostPrefab = GetRandomGhostPrefab();
            Instantiate(randomGhostPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float z = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
        return new Vector3(x, 0, z); // Ensure ghosts are spawned at y = 0
    }

    GameObject GetRandomGhostPrefab()
    {
        int randomIndex = Random.Range(0, ghostPrefabs.Length);
        return ghostPrefabs[randomIndex];
    }
}
