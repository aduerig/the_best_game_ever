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
    private bool hatExpand = false;
    public main mainRef;
    public CharacterLife characterLife;
    public bool IsDead;
    private List<KeyInputType> prevKeysPressed = new List<KeyInputType>();
    
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

        if (animator.GetBool("bigDead"))
        {
            Deactivate();
            animator.SetBool("bigDead", false);
        }

        switch (characterType)
        {
            case CharacterTypes.Barbershop:
                GameObject hatObj =  transform.GetChild(0).gameObject;
                Vector2 scale = hatObj.transform.localScale;
                if(hatExpand && scale.x < 2.5)
                {
                    scale.x += 0.05f;
                    hatObj.transform.localScale = scale;
                }
                else if (!hatExpand && scale.x > 1)
                {
                    scale.x -= 0.05f;
                    hatObj.transform.localScale = scale;
                }
                break;
            default:
                break;
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
        //Debug.Log("object: " + collision.otherCollider.name + ", colliding with: " + collision.collider.name + ", enabled: " + collision.enabled);
        if(collision.enabled) 
        {
            //Debug.Log("NOT GRouNDED LOL");
            ContactPoint2D[] contactPointsPopulate = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contactPointsPopulate);
            // collision.contactCount
            foreach (ContactPoint2D contact in contactPointsPopulate)
            {
                if (contact.normal.y > 0.9)
                {
                    if(collision.collider.tag == "Bouncy")
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 16);
                    }
                    else if (collision.otherCollider.tag != "NoJump")
                    {
                        isGrounded = true;

                        bool cycleRide = false;
                        CharacterController tmpCharacter = collision.gameObject.GetComponent<CharacterController>();
                        while (tmpCharacter != null && tmpCharacter.ride != null) 
                        {
                            if (tmpCharacter.ride == gameObject)
                            {
                                cycleRide = true;
                                break;
                            }
                            tmpCharacter = tmpCharacter.ride.GetComponent<CharacterController>();
                        }

                        if (!cycleRide)
                        {
                            //Debug.Log("object: " + collision.otherCollider.name + ", colliding with: " + collision.collider.name);
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

    public void Kill()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        animator.SetTrigger("isDead");
        IsDead = true;
    }

    public void Resurrect()
    {
        gameObject.SetActive(true);
        IsDead = false;
        hasKey = false;
        isInDoor = false;
        hatExpand = false;

        prevKeysPressed = new List<KeyInputType>();

        GetComponent<Rigidbody2D>().gravityScale = 4;

        if (characterType == CharacterTypes.Barbershop)
        {
            var child = gameObject.transform.Find("Hat");
            if (child != null)
            {
                Vector2 scale = child.transform.localScale;
                scale.x = 1;
                child.transform.localScale = scale;
            }
        }
    }

    public void takeActions(List<KeyInputType> keysPressed, float horizontal)
    {
        if (!IsDead)
        {
            if (ride) 
            {
                rideVelocity = ride.GetComponent<Rigidbody2D>().velocity;
            }
            Vector2 newVel = new Vector2(horizontal * 5 + rideVelocity.x, GetComponent<Rigidbody2D>().velocity.y);

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
        prevKeysPressed = keysPressed;
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
                if(!prevKeysPressed.Contains(KeyInputType.Action)){
                    hatExpand = !hatExpand;
                }
                break;
            default:

                break;
        }
        
    }
}
