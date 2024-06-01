using UnityEngine;
using TMPro; // Add this namespace for TextMeshPro

public class GemManager : MonoBehaviour
{
    public static GemManager Instance { get; private set; }
    public TMP_Text gemText; // Reference to the TextMeshPro Text component
    private int gemCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the GemManager persists across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance of GemManager
        }
    }

    private void Start()
    {
       
        UpdateGemText();
    }

    public void CollectGem()
    {
        gemCount++;
        UpdateGemText();
    }

    private void UpdateGemText()
    {
        gemText.text = gemCount.ToString();
    }
}
