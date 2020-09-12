using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    public GameObject temp_char;
    private GameObject curr_char;

    void Start()
    {
        
    }

    void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            curr_char = Instantiate(temp_char);
        }
        
        if(curr_char){
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 position = curr_char.transform.position;
            position.x = position.x + 5.0f * horizontal * Time.deltaTime;
            position.y = position.y + 5.0f * vertical * Time.deltaTime;
            curr_char.transform.position = position;
        }
        
    }
}
