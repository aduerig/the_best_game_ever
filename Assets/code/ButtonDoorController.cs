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
        var animators = gameObject.GetComponentsInParent<Animator>();
        foreach (var animator in animators)
        {
            animator.SetBool("isPushed", true);
        }
        doorObj.GetComponent<BoxCollider2D>().enabled = false; // TODO: does this open all doors?
        
    }
}
