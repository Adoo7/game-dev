using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    Rigidbody rb;
    [SerializeField] string tagToIgnore;
    // Start is called before the first frame update
    Vector3 forwardPos;
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        forwardPos = Camera.main.transform.forward;
        Invoke("DestroyBullet", 2f);

    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(bulletSpeed * Time.deltaTime, 0, 0); //move the bullet forward continuously

        if (PlayerMovementController.Instance.phase2)
        {
            if(tagToIgnore == "Player") //if its player's bullet then go with camera's forward vector   
            {
                rb.velocity =  forwardPos * bulletSpeed;
            }
            else
            {
                rb.velocity = transform.forward * bulletSpeed;
            }
        }
        else
        {
            rb.velocity = transform.forward * bulletSpeed;
        }


    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == tagToIgnore) return; //ignore trigger with the character shooting it
        if(other.tag == "Enemy")
        {
            Boss.Instance.hit();
        }
        else if(other.tag == "Player")
        {
            Health.Instance.hit();
        }
        else if(other.tag == "Ghost")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject); //destroy bullet when it hits anything
    }
}
