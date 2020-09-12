using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuigiController : MonoBehaviour
{
    public bool isGrounded = false;
    private Collision2D groundedCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("how does this work");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Output the Collider's GameObject's name
        // Debug.Log("enter: " + collision.collider.name);
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if(contact.normal == Vector2.up){
                isGrounded = true;
                groundedCollider = collision;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision == groundedCollider)
        {
            //isGrounded = false;
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
