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
