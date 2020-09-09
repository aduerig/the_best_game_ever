using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuigiController : MonoBehaviour
{
    int random_int = 0;
    float increment = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;

        if (random_int > 100)
        {
            random_int = 0;
            increment *= -1;
        }        
        position.x = position.x + increment;
        transform.position = position;
        random_int += 1;
    }
}
