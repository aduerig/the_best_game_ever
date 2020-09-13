using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum CharacterTypes
{
    Luigi,
    Barbershop,
    Lincoln,
    Grompy
}

public class CharacterController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public CharacterTypes characterType = CharacterTypes.Luigi;
    public bool isGrounded = false;
    public bool hasKey = false;
    public bool isInDoor = false;

    public GameObject ride = null;
    private Vector2 rideVelocity = Vector2.zero;
    private bool hatExpanding = true;
    public main mainRef;
    public CharacterLife characterLife;
    
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
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (isGrounded && collision.gameObject == ride) {
            isGrounded = false;
            ride = null;
            //transform.SetParent(null);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(!isGrounded) 
        {
            
            //Debug.Log("NOT GRouNDED LOL");
            ContactPoint2D[] contactPointsPopulate = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contactPointsPopulate);
            // collision.contactCount
            foreach (ContactPoint2D contact in contactPointsPopulate)
            {
                //Debug.Log("current normal: " + contact.normal + ", Vector2.up: " + Vector2.up + ", contact.normal == Vector2.up: " + (bool) (contact.normal == Vector2.up));
                if (contact.normal.y > 0.9)
                {
                    //Debug.Log("first: " + collision.collider + ", second: " + collision.otherCollider);
                    if(collision.collider.tag == "Bouncy")
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 15);
                    }
                    else if (collision.otherCollider.tag != "NoJump")
                    {
                        isGrounded = true;

                        bool cycleRide = false;
                        CharacterController tmpCharacter = collision.gameObject.GetComponent<CharacterController>();
                        while(tmpCharacter != null && tmpCharacter.ride != null){
                            if(tmpCharacter.ride == gameObject){
                                cycleRide = true;
                                break;
                            }
                            tmpCharacter = tmpCharacter.ride.GetComponent<CharacterController>();
                        }

                        if(!cycleRide){
                            ride = collision.gameObject;
                        }
                    }
                }
            }
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void takeActions(List<KeyInputType> keysPressed, float horizontal)
    {
        if (ride) 
        {
            rideVelocity = ride.GetComponent<Rigidbody2D>().velocity;
        }
        Vector2 newVel = new Vector2(horizontal * 5 + rideVelocity.x, GetComponent<Rigidbody2D>().velocity.y);


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
            GetComponent<Rigidbody2D>().velocity = currentVel;
        }
        */
        if (!isInDoor)
        {
            foreach (KeyInputType keyPressed in keysPressed)
            {
                if (isGrounded && keyPressed == KeyInputType.Jump)
                {
                    newVel.y = 10;
                    isGrounded = false;
                    ride = null;
                    //transform.SetParent(null);
                }
                if (keyPressed == KeyInputType.Action)
                {
                    doCharacterAction();
                }
            }
            //Debug.Log(newVel);
            GetComponent<Rigidbody2D>().velocity = newVel;
        }
    }

    void doCharacterAction()
    {
        Vector2 scale;
        switch (characterType)
        {
            case CharacterTypes.Luigi:
                scale = transform.localScale;
                scale.x = scale.x * 0.5f;
                scale.y = scale.y * 0.5f;
                transform.localScale = scale;
                break;
            case CharacterTypes.Barbershop:
                GameObject hatObj =  transform.GetChild(0).gameObject;
                scale = hatObj.transform.localScale;
                if (hatExpanding)
                {
                    scale.x = scale.x + 0.05f;
                    hatObj.transform.localScale = scale;
                    if (scale.x > 2.5)
                    {
                        hatExpanding = false;
                    }
                }
                else
                {
                    scale.x -= 0.05f;
                    hatObj.transform.localScale = scale;
                    if (scale.x < 1)
                    {
                        hatExpanding = true;
                    }
                }
                break;
            default:

                break;
        }
        
    }
}
