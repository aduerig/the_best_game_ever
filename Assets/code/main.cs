using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class HelperMethods
{
    public static void updatePos(GameObject gameObject, float horizontal, float vertical)
    {
        Vector2 position = gameObject.transform.position;
        position.x = position.x + 5.0f * horizontal * Time.deltaTime;
        position.y = position.y + 5.0f * vertical * Time.deltaTime;
        gameObject.transform.position = position;
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
        }
        else if (currCharacter) 
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            HelperMethods.updatePos(currCharacter, horizontal, vertical);
            currCharacterLife.TrackInput(Time.deltaTime, horizontal, vertical);

            foreach (CharacterLife life in characterLives)
            {
                life.UpdateFromHistory(Time.deltaTime);
            }
        }        
    }
}

public enum InputType
{
  Left,
  Right,
  Jump
}


public class CharacterLife
{
    public List<Tuple<float, float, float>> inputs;
    private int currentPositionInArray;
    private GameObject unityObject;
    Vector3 initTransformPosition; 

    public CharacterLife(GameObject obj)
    {
        inputs = new List<Tuple<float, float, float>>();
        currentPositionInArray = 0;
        unityObject = obj;
        initTransformPosition = unityObject.transform.position;
    }

    public void TrackInput(float timeDelta, float horizontal, float vertical)
    {
        inputs.Add(new Tuple<float, float, float>(timeDelta, horizontal, vertical));
    }

    public void UpdateFromHistory(float timeDelta)
    {
        if (currentPositionInArray < inputs.Count)
        {
            HelperMethods.updatePos(unityObject, 
                inputs[currentPositionInArray].Item2, 
                inputs[currentPositionInArray].Item3
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