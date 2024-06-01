using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float speed = 7f; // Speed of the ghost
    public float rotationSpeed = 4f; // Speed at which the ghost rotates
    public float neighborDistance = 10f; // Distance within which neighbors are detected
    public float separationDistance = 5f; // Minimum distance to maintain from neighbors
    public float followRadius = 20f; // Distance within which the ghost follows the player

    private Transform player; // Reference to the player Transform
    private Vector3 direction; // Current direction of the ghost
    private List<GameObject> neighbors; // List of neighboring ghosts
    private Vector3 initialPosition; // The initial position of the ghost
    private bool isFollowingPlayer; // Whether the ghost is following the player
    private bool isPlayerNearby; // Whether the player is nearby

    void Start()
    {
        direction = Vector3.zero; // Initially still
        initialPosition = transform.position; // Save the initial position

        // Get the player reference from the singleton
        player = PlayerSingleton.Instance.playerTransform;

        // Check if the player reference is assigned
        if (player == null)
        {
            Debug.LogError($"{gameObject.name} does not have a player assigned!");
        }
    }

    void Update()
    {
        if (player == null)
        {
            // Early exit if the player reference is not assigned
            return;
        }

        neighbors = GetNeighbors(); // Update the list of neighbors
        isPlayerNearby = IsPlayerInFollowRadius();

        if (isPlayerNearby || IsNeighborFollowingPlayer())
        {
            Vector3 follow = FollowPlayer(); // Calculate the follow vector
            if (follow != Vector3.zero) // If the follow vector is not zero, prioritize following the player
            {
                direction = Vector3.Lerp(direction, follow, rotationSpeed * Time.deltaTime).normalized;
                isFollowingPlayer = true;
            }
            else
            {
                isFollowingPlayer = false;
            }
        }
        else
        {
            isFollowingPlayer = false;
            direction = Vector3.zero; // Stop moving if player is not nearby and no neighbors are following
        }

        if (isFollowingPlayer) // Only apply flocking behavior if following the player
        {
            Vector3 separation = Separation() * 1.5f; // Calculate separation vector
            Vector3 alignment = Alignment() * 1.0f; // Calculate alignment vector
            Vector3 cohesion = Cohesion() * 1.0f; // Calculate cohesion vector

            Vector3 flockingDirection = separation + alignment + cohesion; // Combine flocking behaviors

            if (flockingDirection != Vector3.zero)
            {
                direction = Vector3.Lerp(direction, flockingDirection, rotationSpeed * Time.deltaTime).normalized;
            }
        }
        else
        {
            // Allow slight movement for animation purposes when not following
            float animationOffset = Mathf.Sin(Time.time * 2.0f) * 0.1f; // Adjust frequency and amplitude as needed
            transform.position = initialPosition + new Vector3(0, animationOffset, 0);
            return;
        }

        // Ensure movement is horizontal
        direction.y = 0;

        // Move the ghost in the calculated direction if it's not zero
        if (direction != Vector3.zero)
        {
            transform.position += direction * speed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z); // Keep ghosts at the initial height
            transform.rotation = Quaternion.LookRotation(direction); // Rotate the ghost to face the direction
        }
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
        return neighbors;
    }

    // Check if the player is within the follow radius
    bool IsPlayerInFollowRadius()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer < followRadius;
    }

    // Check if any neighbor is following the player
    bool IsNeighborFollowingPlayer()
    {
        foreach (var neighbor in neighbors)
        {
            Ghost neighborGhost = neighbor.GetComponent<Ghost>();
            if (neighborGhost != null && neighborGhost.isFollowingPlayer)
            {
                return true;
            }
        }
        return false;
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
            return directionToCenter;
        }
        return Vector3.zero;
    }

    // Calculate the follow vector to steer towards the player if within follow radius
    Vector3 FollowPlayer()
    {
        if (isPlayerNearby)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0; // Ensure movement is horizontal
            directionToPlayer.Normalize();
            return directionToPlayer;
        }
        return Vector3.zero;
    }
}
