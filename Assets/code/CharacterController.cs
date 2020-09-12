using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public bool isGrounded = false;

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
}
