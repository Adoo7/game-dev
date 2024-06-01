using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public LayerMask interactableLayermask = 6;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3, interactableLayermask))
        {
            // if (hit.collider.gameObject.tag == "Interactable")
            // {
            //     Debug.Log("Interactable object found");
            // }
            Debug.Log("OBJECT FOUND");
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
