using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab; // The ghost prefab to spawn
    public int numberOfGhosts = 10; // The number of ghosts to spawn
    public float spawnRadius = 20f; // The radius within which to spawn the ghosts
    public float spawnHeight = 0.5f; // The height at which to spawn the ghosts

    void Start()
    {
        SpawnGhosts();
    }

    void SpawnGhosts()
    {
        for (int i = 0; i < numberOfGhosts; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Debug.Log($"Spawning ghost at position: {spawnPosition}");
            Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
        randomPosition.y = spawnHeight; // Set the spawn height to a fixed value above the ground
        randomPosition += transform.position;
        return randomPosition;
    }
}
