using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Attach player to the platform
    void OnCollisionEnter2D(Collision2D other) 
    {
            if (other.gameObject.tag == "Player"){
                other.gameObject.transform.parent = this.transform;
            }
    }

    // Dettach player to the platform
    void OnCollisionExit2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player") {
            other.gameObject.transform.parent = null;
        }
    }
}
