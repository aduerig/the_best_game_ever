using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var holder = other.GetComponent<CharacterController>();
            if(holder.hasKey)
            {
                gameObject.GetComponent<Animator>().SetBool("isPushed", true);
                var boxes = gameObject.GetComponents<BoxCollider2D>();
                foreach (var box in boxes)
                {
                    box.enabled = false;
                }
                other.transform.Find("Key").gameObject.SetActive(false);
            }
        }

    }

    public void LockObject()
    {
        gameObject.GetComponent<Animator>().SetBool("isPushed", false);
        var boxes = gameObject.GetComponents<BoxCollider2D>();
        foreach (var box in boxes)
        {
            box.enabled = true;
        }
    }
}
