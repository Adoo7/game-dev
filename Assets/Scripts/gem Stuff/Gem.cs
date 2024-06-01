using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public float respawnTime = 5f; // Time after which the gem respawns
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
        transform.position = GetRandomPosition();
        gameObject.SetActive(true);
    }

    private Vector3 GetRandomPosition()
    {
        // Generate a random position within a certain area
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, initialPosition.y, z);
    }
}
