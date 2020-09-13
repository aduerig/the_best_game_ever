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
    private Vector2 rideVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vect = GetComponent<Rigidbody2D>().velocity;

        animator.SetFloat("horizontal", Mathf.Abs(Input.GetAxis("Horizontal")));
        animator.SetFloat("vertical", Input.GetAxis("Vertical"));

        if (vect.x < -.05)
        {
            spriteRenderer.flipX = true;
        }
        else if (vect.x > .05)
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

        /*
        Vector2 currVel = GetComponent<Rigidbody2D>().velocity;
        Vector2 collVel = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if(contact.normal == Vector2.right && currVel.x > 0){
                currVel.x = 0;
                collVel.x = 0;
                Debug.Log("push right");
                GetComponent<Rigidbody2D>().velocity = currVel;
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = collVel;
            }else if(contact.normal == Vector2.left && currVel.x < 0){
                currVel.x = 0;
                collVel.x = 0;
                Debug.Log("push left");
                GetComponent<Rigidbody2D>().velocity = currVel;
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = collVel;
            }
        }
        */
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(!isGrounded){
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if(contact.normal == Vector2.up){
                    isGrounded = true;
                    ride = collision.gameObject;
                    //Debug.Log(ride);
                }
            }
        }
    }

    public void takeActions(GameObject gameObject, List<KeyInputType> keysPressed, float horizontal)
    {
        if(ride){
            rideVelocity = ride.GetComponent<Rigidbody2D>().velocity;
        }
        Vector2 newVel = new Vector2(horizontal * 10 + rideVelocity.x, GetComponent<Rigidbody2D>().velocity.y);
        

        /*

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
        */
        foreach (KeyInputType keyPressed in keysPressed)
        {
            if (isGrounded && keyPressed == KeyInputType.Jump)
            {
                newVel.y = 18;
                isGrounded = false;
                ride = null;
            }
            if (keyPressed == KeyInputType.Action)
            {
                HelperMethods.doCharacterAction(gameObject, CharacterTypes.Luigi);// TODO: this should call character's action method
            }
        }
        //Debug.Log(newVel);
        GetComponent<Rigidbody2D>().velocity = newVel;
    }
}
