using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
        // If player, just fuckin obliterate em
        if (other.collider.tag == "Player")
        {
            var yaBoy = other.collider.GetComponent<CharacterController>();
            yaBoy.Deactivate();
            //yaBoy.mainRef.EnterTheShadowRealm();
        }
    }
}
