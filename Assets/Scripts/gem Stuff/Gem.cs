using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public float respawnTime = 5f; // Time after which the gem respawns
    public float respawnRadius = 10f; // Radius within which the gem can respawn
    private Vector3 initialPosition; // Initial position of the gem

    void Start()
    {
        initialPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered by: {other.gameObject.name} with tag: {other.tag}");
        if (other.CompareTag("Player"))
        {
            // Add gem to the player's count
            GemManager.Instance.CollectGem();
            Debug.Log("Gem collected");
            // Hide the gem and start respawn coroutine
            StartCoroutine(RespawnGem());
        }
    }

    private IEnumerator RespawnGem()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnTime);
        transform.position = GetRandomPositionWithinRadius();
        gameObject.SetActive(true);
    }

    private Vector3 GetRandomPositionWithinRadius()
    {
        // Generate a random position within a certain radius around the initial position
        Vector3 randomDirection = Random.insideUnitSphere * respawnRadius;
        randomDirection.y = 0; // Keep the position at the same height
        Vector3 randomPosition = initialPosition + randomDirection;

        // Ensure the position is within a certain range to avoid extreme positions
        randomPosition.x = Mathf.Clamp(randomPosition.x, initialPosition.x - respawnRadius, initialPosition.x + respawnRadius);
        randomPosition.z = Mathf.Clamp(randomPosition.z, initialPosition.z - respawnRadius, initialPosition.z + respawnRadius);

        return randomPosition;
    }
}
