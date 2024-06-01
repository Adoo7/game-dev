using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float speed = 7f; // Speed of the ghost
    public float rotationSpeed = 4f; // Speed at which the ghost rotates
    public float neighborDistance = 10f; // Distance within which neighbors are detected
    public float separationDistance = 5f; // Minimum distance to maintain from neighbors
    public float followRadius = 20f; // Distance within which the ghost follows the player
    public GameObject player; // Reference to the player object

    private Vector3 direction; // Current direction of the ghost
    private List<GameObject> neighbors; // List of neighboring ghosts

    void Start()
    {
        direction = transform.forward; // Initialize direction to the forward direction
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        neighbors = GetNeighbors(); // Update the list of neighbors
        Vector3 follow = FollowPlayer() * 2.0f; // Calculate the follow vector

        if (follow != Vector3.zero) // If the follow vector is not zero, prioritize following the player
        {
            Debug.Log($"{gameObject.name} is following the player.");
            direction = Vector3.Lerp(direction, follow, rotationSpeed * Time.deltaTime).normalized;
        }
        else // If not following the player, use flocking behavior
        {
            Vector3 separation = Separation() * 1.5f; // Calculate separation vector
            Vector3 alignment = Alignment() * 1.0f; // Calculate alignment vector
            Vector3 cohesion = Cohesion() * 1.0f; // Calculate cohesion vector

            Debug.Log($"{gameObject.name} is flocking. Separation: {separation}, Alignment: {alignment}, Cohesion: {cohesion}");
            Vector3 flockingDirection = separation + alignment + cohesion; // Combine flocking behaviors
            direction = Vector3.Lerp(direction, flockingDirection, rotationSpeed * Time.deltaTime).normalized;
        }

        direction.y = 0; // Ensure movement is horizontal

        // Move the ghost in the calculated direction
        transform.position += direction * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z); // Keep ghosts at a fixed height
        transform.rotation = Quaternion.LookRotation(direction); // Rotate the ghost to face the direction
    }

    // Get a list of neighboring ghosts within the detection radius
    List<GameObject> GetNeighbors()
    {
        List<GameObject> neighbors = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, neighborDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && hitCollider.gameObject.CompareTag("Ghost"))
            {
                neighbors.Add(hitCollider.gameObject);
            }
        }
        Debug.Log($"{gameObject.name} found {neighbors.Count} neighbors.");
        return neighbors;
    }

    // Calculate the separation vector to avoid crowding local flockmates
    Vector3 Separation()
    {
        Vector3 steer = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            float distance = Vector3.Distance(transform.position, neighbor.transform.position);
            if (distance < separationDistance)
            {
                Vector3 diff = transform.position - neighbor.transform.position;
                diff.Normalize();
                diff /= distance;
                steer += diff;
            }
        }
        Debug.Log($"{gameObject.name} separation steer: {steer}");
        return steer;
    }

    // Calculate the alignment vector to steer towards the average heading of local flockmates
    Vector3 Alignment()
    {
        Vector3 averageDirection = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            averageDirection += neighbor.transform.forward;
        }
        if (neighbors.Count > 0)
        {
            averageDirection /= neighbors.Count;
            averageDirection.Normalize();
        }
        Debug.Log($"{gameObject.name} alignment direction: {averageDirection}");
        return averageDirection;
    }

    // Calculate the cohesion vector to steer towards the average position of local flockmates
    Vector3 Cohesion()
    {
        Vector3 averagePosition = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            averagePosition += neighbor.transform.position;
        }
        if (neighbors.Count > 0)
        {
            averagePosition /= neighbors.Count;
            Vector3 directionToCenter = averagePosition - transform.position;
            directionToCenter.Normalize();
            Debug.Log($"{gameObject.name} cohesion direction: {directionToCenter}");
            return directionToCenter;
        }
        return Vector3.zero;
    }

    // Calculate the follow vector to steer towards the player if within follow radius
    Vector3 FollowPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < followRadius)
            {
                Vector3 directionToPlayer = player.transform.position - transform.position;
                directionToPlayer.y = 0; // Ensure movement is horizontal
                directionToPlayer.Normalize();
                Debug.Log($"{gameObject.name} is within follow radius. Direction to player: {directionToPlayer}");
                return directionToPlayer;
            }
        }
        return Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //transform.Translate(Vector3.back); //move the ghost a bit back after hitting the player
            Health.Instance.hit();
        }
    }
}
