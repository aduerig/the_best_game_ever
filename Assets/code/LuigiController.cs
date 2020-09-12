using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuigiController : MonoBehaviour
{
    public bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("how does this work");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Output the Collider's GameObject's name
        // Debug.Log("enter: " + collision.collider.name);
        if (collision.collider.name == "ground2")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "ground2")
        {
            isGrounded = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {

        // Debug.Log(collision.collider.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
