using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    Animator myAnimator;
    [SerializeField] Transform player;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float speed;
    public bool phase2;
    [SerializeField] GameObject bulletInHand;

    public static Boss Instance; //singleton so the class can be referenced by anything
    public float health = 100;
    public Image healthBar;
    public Text healthText;


    public GameObject ghost;

    public GameObject winPanel;

    // Start is called before the first frame update
    void Start()
    {

        Instance = this;
        myAnimator = GetComponent<Animator>();
        transform.LookAt(player);
        InvokeRepeating("Shoot", 2f, 2f); //shoot once every 2 seconds
        
    }

    // Update is called once per frame
    void Update()
    {
        if (phase2 && GameObject.FindGameObjectsWithTag("Ghost").Length != 0)
        {
            return; 
        }

        if (Health.Instance.health == 0) return;
        if (phase2)
        {
            transform.LookAt(player);
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w); //lock unnecessary rotation on X axis
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (3f < distance && distance < 4f) //be idle if distance is between 3 and 4
        {
            myAnimator.SetBool("idle", true);
        }
        else if (distance > 4f) // go forward if distance is more than 4
        {
            transform.Translate(-transform.forward * speed * Time.deltaTime);

            myAnimator.SetBool("idle", false);
            myAnimator.SetBool("forward", true);
        }
        else if (distance < 3f)  //back up if distance is less than 3
        {
            transform.Translate(transform.forward * speed * Time.deltaTime);
            myAnimator.SetBool("idle", false);
            myAnimator.SetBool("forward", false);
        }

        healthBar.fillAmount = health / 100;
        healthText.text = health.ToString();



    }

    public void Shoot()
    {
        if (phase2 && GameObject.FindGameObjectsWithTag("Ghost").Length != 0) return; //dont shoot if ghosts are active in second phase
            bulletInHand.SetActive(true);
            myAnimator.SetTrigger("throw");
            Invoke("Bullet", 0.5f);
    }

    public void Bullet()
    {
            Instantiate(bulletPrefab, transform.position + Vector3.up, transform.rotation);
            bulletInHand.SetActive(false);
    }

    public void hit()
    {
        if (phase2 && GameObject.FindGameObjectsWithTag("Ghost").Length != 0) return; //dont take damage if ghosts are active
        health -= 10;

        if(health<= 50 && !phase2)
        {
            SpawnGhosts();
            phase2 = true;
            myAnimator.SetBool("idle", true);
            RemoveInvisibleWalls();
        }
        if(health <= 0)
        {
            CancelInvoke("Shoot");
            healthText.text = "0";
            healthBar.fillAmount = 0;

            winPanel.SetActive(true);
            player.GetComponent<InputManager>().DisplayCursor(true);
            CameraManager.stopCam = true;
            player.GetComponent<PlayerMovementController>().enabled = false;
            myAnimator.SetBool("idle", true);
            enabled = false; //disable this script after the boss is dead
            

        }
    }

    public void RemoveInvisibleWalls()
    {
        foreach(GameObject wall in GameObject.FindGameObjectsWithTag("wall"))
        {
            wall.SetActive(false);
        }
    }

    public void SpawnGhosts()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(ghost, transform.position + new Vector3(0,2f, 5f), Quaternion.identity);
        }
    }
}
