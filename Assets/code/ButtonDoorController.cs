using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoorController : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
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
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var doorObj = transform.parent.GetChild(transform.GetSiblingIndex() + 1);
        var animator = gameObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isPushed", false);
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
            animator.SetBool("isPushed", false);
        }
        doorObj.GetComponent<BoxCollider2D>().enabled = true; // TODO: does this open all doors?
    }
}
