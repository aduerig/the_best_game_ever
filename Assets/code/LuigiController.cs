using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuigiController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.collider.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
