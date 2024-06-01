using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Health Instance; //singleton so the class can be referenced by anything

    public int health = 3;
    public GameObject playerHearts;

    public GameObject losePanel;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hit()
    {
        health -= 1;

        playerHearts.transform.GetChild(health).gameObject.SetActive(false);
        if(health == 0)
        {
            losePanel.SetActive(true);
            Boss.Instance.CancelInvoke("Shoot"); //stop the boss from shooting if the player is dead
            GetComponent<InputManager>().DisplayCursor(true);
        }

    }
}
