using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorController : MonoBehaviour
{
    //BoxCollider2D doorCollider;
    Vector2 initKeyPosition;
    Transform origParent;

    LockedObject doorObj;

    // Start is called before the first frame update
    void Start()
    {
        doorObj = transform.parent.Find("Door").GetComponent<LockedObject>();
        initKeyPosition = transform.position;
        origParent = transform.parent;
        //doorCollider = doorObj.GetComponent<BoxCollider2D>
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GetComponent<AudioSource>().Play();

            transform.SetParent(other.transform);
            var collider = other.GetComponent<BoxCollider2D>();
            transform.localPosition = new Vector2(0, collider.size.y);
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            //Destroy(gameObject.GetComponent<BoxCollider2D>());
            /*var doorColliders = doorObj.GetComponents<BoxCollider2D>();
            foreach(var doorCollider in doorColliders)
            {
                doorCollider.enabled = false;
            }*/

            var holder = other.GetComponent<CharacterController>();
            holder.hasKey = true;
        }

    }

    public void ResetKeyDoor()
    {
        transform.SetParent(origParent);
        transform.position = initKeyPosition;
        transform.gameObject.SetActive(true);
        transform.GetComponent<Rigidbody2D>().simulated = true;

        doorObj.LockObject();

        //gameObject.GetComponent<LockedObject>().LockObject();
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
