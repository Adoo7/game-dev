using UnityEngine;
using System.Collections.Generic;

public class GemSpawner : MonoBehaviour
{
    public GameObject gemPrefab; // The gem prefab to spawn
    public int numberOfGems = 10; // The number of gems to spawn
    public Vector3 spawnAreaMin; // The minimum coordinates of the spawn area
    public Vector3 spawnAreaMax; // The maximum coordinates of the spawn area

    private List<GameObject> spawnedGems = new List<GameObject>(); // List to keep track of spawned gems

    void Start()
    {
        SpawnGems();
    }

    void SpawnGems()
    {
        for (int i = 0; i < numberOfGems; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject spawnedGem = Instantiate(gemPrefab, spawnPosition, Quaternion.identity);
            spawnedGems.Add(spawnedGem);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        float z = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
        return new Vector3(x, y, z);
    }

    public void RespawnGem(GameObject gem)
    {
        Vector3 newSpawnPosition = GetRandomSpawnPosition();
        gem.transform.position = newSpawnPosition;
        gem.SetActive(true);
    }
}
