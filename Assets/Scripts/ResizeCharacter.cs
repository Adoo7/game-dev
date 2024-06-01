using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ResizeCharacter : MonoBehaviour {
 
    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
 
        if (Input.GetKeyDown (KeyCode.Alpha1)) {
            transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f);
        } else if (Input.GetKeyDown (KeyCode.Alpha2)) {
            // transform.position = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);
            transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
        } else if (Input.GetKeyDown (KeyCode.Alpha3)) {
            // transform.position = new Vector3 (transform.position.x, transform.position.y + 1.0f, transform.position.z);
            transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
        }
 
    }
}