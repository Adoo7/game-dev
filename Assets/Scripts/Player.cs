using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            // Add gem to the player's count
            GemManager.Instance.CollectGem();
            // Hide the gem and start respawn coroutine
            other.gameObject.SetActive(false);
            StartCoroutine(RespawnGem(other.gameObject));
        }
    }

    private IEnumerator RespawnGem(GameObject gem)
    {
        yield return new WaitForSeconds(5f);
        gem.transform.position = GetRandomPosition(gem.transform.position);
        gem.SetActive(true);
    }

    private Vector3 GetRandomPosition(Vector3 initialPosition)
    {
        // Generate a random position within a certain area
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, initialPosition.y, z);
    }
}
