using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    private GameObject ride = null;
    private Vector2 rideVelocity = Vector2.zero;
    private bool hatExpanding = true;
    
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
        if(ride){
            rideVelocity = ride.GetComponent<Rigidbody2D>().velocity;
        }
        Vector2 newVel = new Vector2(horizontal * 10 + rideVelocity.x, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        

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
                doCharacterAction(gameObject);
            }
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = newVel;
    }

    void doCharacterAction(GameObject gameObject)
    {
        Vector2 scale;
        switch (characterType)
        {
            case CharacterTypes.Luigi:
                scale = gameObject.transform.localScale;
                scale.x = scale.x * 0.5f;
                scale.y = scale.y * 0.5f;
                gameObject.transform.localScale = scale;
                break;
            case CharacterTypes.Barbershop:
                GameObject hatObj = gameObject.transform.GetChild(0).gameObject;
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
