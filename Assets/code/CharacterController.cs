using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool isGrounded = false;
    private GameObject ride = null;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("horizontal", Mathf.Abs(Input.GetAxis("Horizontal")));
        animator.SetFloat("vertical", Input.GetAxis("Vertical"));

        if (Input.GetAxis("Horizontal") < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
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
                ride = collision.gameObject;
                gameObject.GetComponent<Rigidbody2D>().velocity += ride.GetComponent<Rigidbody2D>().velocity;
                Debug.Log(ride);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {

        // Debug.Log(collision.collider.name);
    }

    public void takeActions(GameObject gameObject, List<KeyInputType> keysPressed, float horizontal)
    {
        float addVertVel = 0, addHoriVel = 0;
        Vector2 currentVel = gameObject.GetComponent<Rigidbody2D>().velocity;
        // Debug.Log("velocity: " + currentHoriVel);
        Vector2 rideVelocity = Vector2.zero;
        if(ride){
            rideVelocity = ride.GetComponent<Rigidbody2D>().velocity;
        }

        // hoirzontal movement
        float maxHoriVel = rideVelocity.x + 12;
        float minHoriVel = rideVelocity.x - 12;
        addHoriVel = horizontal * Time.deltaTime * 1000000;

        // cap hoirzontal velocity
        if (currentVel.x + addHoriVel > maxHoriVel)
        {
            addHoriVel = maxHoriVel - currentVel.x;
        }

        else if (currentVel.x + addHoriVel < minHoriVel)
        {
            addHoriVel = minHoriVel - currentVel.x;
        }

        // slow down character if no is pressing button
        if ((!keysPressed.Contains(KeyInputType.Left) && currentVel.x < .01) ||
            (!keysPressed.Contains(KeyInputType.Right) && currentVel.x > -.01))
        {
            currentVel.x *= .92f;
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
        Vector2 force = new Vector2(addHoriVel + rideVelocity.x, addVertVel + rideVelocity.y);
        gameObject.GetComponent<Rigidbody2D>().AddForce(force);
    }
}
