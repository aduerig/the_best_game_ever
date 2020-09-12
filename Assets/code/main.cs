﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class HelperMethods
{
    public static void updateForces(GameObject gameObject, KeyInputType keyPressed, float horizontal)
    {
        // Vector2 position = gameObject.transform.position;
        float jumpVel = 0;
        
        var controllerScript = gameObject.GetComponent<LuigiController>();
        if (controllerScript.isGrounded && keyPressed == KeyInputType.Jump)
        {
            jumpVel = 600;
            controllerScript.isGrounded = false;
        }
        Vector2 force = new Vector2(horizontal * Time.deltaTime * 1000, jumpVel);
        gameObject.GetComponent<Rigidbody2D>().AddForce(force);
        // position.x = position.x + 5.0f * horizontal * Time.deltaTime;
        // position.y = position.y + 5.0f * vertical * Time.deltaTime;
        // gameObject.transform.position = position;
    }

    public static void doCharacterAction(GameObject gameObject, CharacterTypes characterType)
    {
        Vector2 scale = gameObject.transform.localScale;
        switch (characterType)
        {
            case CharacterTypes.Luigi:
                scale.x = scale.x * 0.5f;
                scale.y = scale.y * 0.5f;
                break;
            case CharacterTypes.Barbershop:
                scale.x = scale.x * 1.5f;
                scale.y = scale.y * 1.5f;
                break;
            default:

                break;
        }
        gameObject.transform.localScale = scale;
    }
}

public class main : MonoBehaviour
{
    private GameObject currCharacter;
    private CharacterLife currCharacterLife;

    private GameObject luigiPrefab;
    private List<CharacterLife> characterLives;

    void Start()
    {
        characterLives = new List<CharacterLife>();
        luigiPrefab = Resources.Load<GameObject>("very_important_asset");
        // Debug.Log("truly what");
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currCharacter != null && currCharacterLife != null)
            {
                characterLives.Add(currCharacterLife);
                foreach (CharacterLife life in characterLives)
                {
                    life.ResetToSpawn();
                }
            }
            currCharacter = Instantiate(luigiPrefab);
            currCharacterLife = new CharacterLife(currCharacter);
            currCharacterLife.characterType = CharacterTypes.Luigi;
        }
        else if (currCharacter)
        {
            KeyInputType keyPressed = KeyInputType.None;
            if (Input.GetKey("up"))
            {
                keyPressed = KeyInputType.Jump;
            }
            if (Input.GetKey("left"))
            {
                keyPressed = KeyInputType.Left;
            }
            if (Input.GetKey("right"))
            {
                keyPressed = KeyInputType.Right;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                keyPressed = KeyInputType.Action;
                HelperMethods.doCharacterAction(currCharacter, currCharacterLife.characterType);
            }

            float horizontal = Input.GetAxis("Horizontal");
            // float vertical = Input.GetAxis("Vertical");
            HelperMethods.updateForces(currCharacter, keyPressed, horizontal);
            currCharacterLife.TrackInput(Time.deltaTime, keyPressed, horizontal);

            foreach (CharacterLife life in characterLives)
            {
                life.UpdateFromHistory(Time.deltaTime);
            }
        }
    }
}

public enum KeyInputType
{
  Left,
  Right,
  Jump,
  Action,
  None
}

public enum CharacterTypes
{
    Luigi,
    Barbershop
}


public class CharacterLife
{
    public CharacterTypes characterType;
    public List<Tuple<float, KeyInputType, float>> history;
    private int currentPositionInArray;
    private GameObject unityObject;
    Vector3 initTransformPosition; 

    public CharacterLife(GameObject obj)
    {
        history = new List<Tuple<float, KeyInputType, float>>();
        currentPositionInArray = 0;
        unityObject = obj;
        initTransformPosition = unityObject.transform.position;
    }

    public void TrackInput(float timeDelta, KeyInputType keyPressed, float horizontal)
    {
        history.Add(new Tuple<float, KeyInputType, float>(timeDelta, keyPressed, horizontal));
    }

    public void UpdateFromHistory(float timeDelta)
    {
        if (currentPositionInArray < history.Count)
        {
            HelperMethods.updateForces(unityObject, 
                history[currentPositionInArray].Item2, 
                history[currentPositionInArray].Item3
            );
            currentPositionInArray++;
        }
    }

    public void ResetToSpawn()
    {
        unityObject.transform.position = initTransformPosition;
        currentPositionInArray = 0;
    }
}