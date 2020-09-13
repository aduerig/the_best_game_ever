using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public int requiredTotalCharacters = 1;
    public int currentTotalCharacters;

    private List<CharacterController> charactersInDoor;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentTotalCharacters = 0;
        charactersInDoor = new List<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("HoldLastWalkthroughFrame"))
        {
            foreach(var character in charactersInDoor)
            {
                character.gameObject.SetActive(false);
            }
            animator.SetBool("characterIn", false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            animator.SetBool("characterIn", true);
            currentTotalCharacters += 1;

            var charController = collision.GetComponent<CharacterController>();
            charController.isInDoor = true;
            charactersInDoor.Add(charController);
        }
    }

    public void ResetProgress()
    {
        currentTotalCharacters = 0;
        charactersInDoor.Clear();
    }

    public bool GoalIsMet()
    {
        return requiredTotalCharacters <= currentTotalCharacters;
    }

    /*void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var animator = gameObject.GetComponent<Animator>();
            animator.SetBool("characterIn", true);
            currentTotalCharacters += 1;
        }
    }*/
}