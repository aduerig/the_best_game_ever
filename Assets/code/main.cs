using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class main : MonoBehaviour
{
    public GameObject TempChar;
    private GameObject CurrChar;

    void Start()
    {
        
    }

    void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            CurrChar = Instantiate(TempChar);
        }
        
        if(CurrChar){
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 position = CurrChar.transform.position;
            position.x = position.x + 5.0f * horizontal * Time.deltaTime;
            position.y = position.y + 5.0f * vertical * Time.deltaTime;
            CurrChar.transform.position = position;
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
    public List<Tuple<double, InputType>> inputs;
    private int currentPositionInArray;
    private GameObject unityObject;

    public CharacterLife(GameObject unityObject)
    {
        inputs = new List<Tuple<double, InputType>>();
        currentPositionInArray = 0;
        unityObject = unityObject;
    }

    public void TrackInput(double timeDelta, InputType input)
    {
        inputs.Add(new Tuple<double, InputType>(timeDelta, input));
    }

    public void UpdateFromHistory(double timeDelta)
    {
        //
    }
}