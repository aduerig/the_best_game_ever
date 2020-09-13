using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorController : MonoBehaviour
{
    Vector2 initPosition;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            transform.SetParent(other.transform);
            var collider = other.GetComponent<BoxCollider2D>();
            transform.localPosition = new Vector2(0, collider.size.y);
            //gameObject.GetComponent<Rigidbody2D>();
            //Destroy(gameObject.GetComponent<BoxCollider2D>());
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            var holder = other.GetComponent<CharacterController>();
            holder.hasKey = true;
        }

    }

    /*void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Player")
        {

        }

        var doorObj = transform.parent.GetChild(transform.GetSiblingIndex() + 1);
        var animator = gameObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isPushed", true);
        }
        var spriteSize = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        var boxes = gameObject.GetComponents<BoxCollider2D>();
        foreach (var box in boxes)
        {
            if (!box.isTrigger)
            {
                box.size = spriteSize;
                box.offset = new Vector2(0, 0.5f - (1 - spriteSize.y) / 2);
            }
        }

        animator = doorObj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isPushed", true);
        }
        doorObj.GetComponent<BoxCollider2D>().enabled = false; // TODO: does this open all doors?

    }*/
}
