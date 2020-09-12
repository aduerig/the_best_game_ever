using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    public GameObject main_char;

    void Start()
    {
        
    }

    void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Instantiate(main_char);
        }


    }
}

enum InputType 
{
  Left,
  Right,
  Jump
}


public class CharacterLife
{
    public ArrayList<Tuple<double, InputType>> inputs;
    private int currentPositionInArray;
    private GameObject unityObject;

    public CharacterLife(GameObject unityObject)
    {
        inputs = new ArrayList<int>();
        currentPositionInArray = 0;
        unityObject = unityObject;
    }

    public TrackInput(double timeDelta, InputType input)
    {
        inputs.Add(new Tuple<double, InputType>(timeDelta, input));
    }

    public UpdateFromHistory(double timeDelta)
    {
        //
    }
}