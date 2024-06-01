using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smokescript : MonoBehaviour
{
    public GameObject player;
    public float duration = 5f;
    private Material originalMaterial;
    private Material invisibleMaterial;
    private bool isPlayerInvisible = false;
    private float timer = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player && !isPlayerInvisible)
        {
            SetPlayerInvisible();
            timer = 0f;
        }
    }

    void Update()
    {
        if (isPlayerInvisible && timer < duration)
        {
            timer += Time.deltaTime;
        }
        else if (isPlayerInvisible && timer >= duration)
        {
            SetPlayerVisible();
            Destroy(gameObject); // Destroy the smoke GameObject
        }
    }

    private void SetPlayerInvisible()
    {
        if (player != null)
        {
            Renderer playerRenderer = player.GetComponent<Renderer>();

            if (originalMaterial == null)
            {
                originalMaterial = playerRenderer.material;
            }

            if (invisibleMaterial == null)
            {
                invisibleMaterial = new Material(Shader.Find("Transparent/Diffuse"));
                Color invisibleColor = originalMaterial.color;
                invisibleColor.a = 0.0f;
                invisibleMaterial.color = invisibleColor;
            }

            playerRenderer.material = invisibleMaterial;
            isPlayerInvisible = true;
        }
    }

    private void SetPlayerVisible()
    {
        if (player != null && originalMaterial != null)
        {
            Renderer playerRenderer = player.GetComponent<Renderer>();

            playerRenderer.material = originalMaterial;
        }
    }
}