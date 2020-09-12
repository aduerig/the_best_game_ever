using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class HelperMethods
{
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
    private GameObject nextCharacter;
    private CharacterLife currCharacterLife;

    public Texture2D lincolnIcon;
    public Texture2D grumpyIcon;

    private GameObject luigiPrefab;
    private GameObject lincolnPrefab;
    private GameObject grumpyPrefab;

    private List<CharacterLife> characterLives;

    void Start()
    {
        characterLives = new List<CharacterLife>();
        luigiPrefab = Resources.Load<GameObject>("very_important_asset");
        lincolnPrefab = Resources.Load<GameObject>("LincolnPrefab");
        grumpyPrefab = Resources.Load<GameObject>("GrumpyPrefab");
        // Debug.Log("truly what");

        nextCharacter = lincolnPrefab;
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

            //currCharacter = Instantiate(luigiPrefab);
            currCharacter = Instantiate(nextCharacter);
            currCharacterLife = new CharacterLife(currCharacter);
            currCharacterLife.characterType = CharacterTypes.Luigi;
        }
        else if (currCharacter)
        {
            List<KeyInputType> keysPressed = new List<KeyInputType>();
            //KeyInputType keyPressed = KeyInputType.None;
            if (Input.GetKey("up"))
            {
                keysPressed.Add(KeyInputType.Jump);
                //keyPressed = KeyInputType.Jump;
            }
            if (Input.GetKey("left"))
            {
                keysPressed.Add(KeyInputType.Left);
                //keyPressed = KeyInputType.Left;
            }
            if (Input.GetKey("right"))
            {
                keysPressed.Add(KeyInputType.Right);
                //keyPressed = KeyInputType.Right;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                keysPressed.Add(KeyInputType.Action);
                //keyPressed = KeyInputType.Action;
                //HelperMethods.doCharacterAction(currCharacter, currCharacterLife.characterType);
            }

            float horizontal = Input.GetAxis("Horizontal");
            var controllerScript = currCharacter.GetComponent<CharacterController>();
            controllerScript.takeActions(currCharacter, keysPressed, horizontal);
            currCharacterLife.TrackInput(Time.deltaTime, keysPressed, horizontal);

            foreach (CharacterLife life in characterLives)
            {
                life.UpdateFromHistory(Time.deltaTime);
            }
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), lincolnIcon))
        {
            nextCharacter = lincolnPrefab;
            print("EMANCIPATION TIME");
        }

        if (GUI.Button(new Rect(10, 70, 100, 50), grumpyIcon))
        {
            nextCharacter = grumpyPrefab;
            print("Feelin' Grompy");
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
    public List<Tuple<float, List<KeyInputType>, float>> history;
    private int currentPositionInArray;
    private GameObject unityObject;
    Vector3 initTransformPosition;
    Vector3 initTransformScale;

    public CharacterLife(GameObject obj)
    {
        history = new List<Tuple<float, List<KeyInputType>, float>>();
        currentPositionInArray = 0;
        unityObject = obj;
        initTransformPosition = unityObject.transform.position;
        initTransformScale = unityObject.transform.localScale;
    }

    public void TrackInput(float timeDelta, List<KeyInputType> keysPressed, float horizontal)
    {
        history.Add(new Tuple<float, List<KeyInputType>, float>(timeDelta, keysPressed, horizontal));
    }

    public void UpdateFromHistory(float timeDelta)
    {
        var controllerScript = unityObject.GetComponent<CharacterController>();
        if (currentPositionInArray < history.Count)
        {
            controllerScript.takeActions(unityObject, 
                history[currentPositionInArray].Item2, 
                history[currentPositionInArray].Item3
            );
            currentPositionInArray++;
        }
    }

    public void ResetToSpawn()
    {
        unityObject.transform.position = initTransformPosition;
        unityObject.transform.localScale = initTransformScale;
        currentPositionInArray = 0;
    }
}