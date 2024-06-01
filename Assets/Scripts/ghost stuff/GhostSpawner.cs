using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab; // The ghost prefab to spawn
    public int numberOfGhosts = 10; // The number of ghosts to spawn
    public float spawnRadius = 20f; // The radius within which to spawn the ghosts
    public Transform ground; // Reference to the ground Transform

    void Start()
    {
        SpawnGhosts();
    }

    void SpawnGhosts()
    {
        float groundHeight = ground.position.y; // Get the ground height
        for (int i = 0; i < numberOfGhosts; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition(groundHeight);
            Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomSpawnPosition(float groundHeight)
    {
        Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;
        randomPosition.y = groundHeight + 1f; // Set the spawn height slightly above the ground
        randomPosition += transform.position;
        return randomPosition;
    }
}
