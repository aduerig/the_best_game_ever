using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LuigiController : MonoBehaviour
{
    public bool isGrounded = false;
    private Collision2D groundedCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("how does this work");
    }

    void FixedUpdate()
    {
        // var vel = GetComponent<Rigidbody2D>().velocity;
        // vel.x *= (float) (1.0f - drag);
        // GetComponent<Rigidbody2D>().velocity = vel;
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

    public void takeActions(GameObject gameObject, List<KeyInputType> keysPressed, float horizontal)
    {
        float addVertVel = 0, addHoriVel = 0;
        Vector2 currentVel = gameObject.GetComponent<Rigidbody2D>().velocity;
        float currentHoriVel = currentVel.x;
        // Debug.Log("velocity: " + currentHoriVel);


        // hoirzontal movement
        float maxHoriVel = 12;
        addHoriVel = horizontal * Time.deltaTime * 1000000;

        // cap hoirzontal velocity
        if (currentHoriVel + addHoriVel > maxHoriVel)
        {
            addHoriVel = maxHoriVel - currentHoriVel;
        }

        else if (currentHoriVel + addHoriVel < -maxHoriVel)
        {
            addHoriVel = -maxHoriVel + currentHoriVel;
        }

        // slow down character if no is pressing button
        if ((!keysPressed.Contains(KeyInputType.Left) && currentVel.x < .1) ||
            (!keysPressed.Contains(KeyInputType.Right) && currentVel.x > -.1))
        {
            currentVel.x *= .94f;
            gameObject.GetComponent<Rigidbody2D>().velocity = currentVel;
        }

        foreach (KeyInputType keyPressed in keysPressed)
        {
            if (isGrounded && keyPressed == KeyInputType.Jump)
            {
                addVertVel = 750;
                isGrounded = false;
            }
            if (keyPressed == KeyInputType.Action)
            {
                HelperMethods.doCharacterAction(gameObject, CharacterTypes.Luigi);// TODO: this should call character's action method
            }
        }
        Vector2 force = new Vector2(horizontal * Time.deltaTime * 1000, addVertVel);
        gameObject.GetComponent<Rigidbody2D>().AddForce(force);
    }
}
