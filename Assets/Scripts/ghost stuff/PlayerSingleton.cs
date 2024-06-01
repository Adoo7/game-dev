using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance { get; private set; }
    public Transform playerTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the player persists across scenes
            playerTransform = transform; // Store the transform for easy access
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance
        }
    }
}
