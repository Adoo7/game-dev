using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shootCooldown = 0.5f; // Time in seconds between shots

    private float lastShotTime;

    // Start is called before the first frame update
    void Start()
    {
        lastShotTime = -shootCooldown; // Ensure the player can shoot immediately at the start
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Health.Instance.health != 0 && Time.time >= lastShotTime + shootCooldown)
        {
            ShootBullet();
            lastShotTime = Time.time;
        }
    }

    void ShootBullet()
    {
        Instantiate(bulletPrefab, transform.position + Vector3.up, Quaternion.identity);
    }
}
