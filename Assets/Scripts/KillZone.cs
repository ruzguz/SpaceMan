using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Collision handler 
    void OnTriggerEnter2D(Collider2D c) 
    {
        
        // Check if the player enter to the kill zone 
        if (c.tag == "Player") {
            PlayerController player = c.GetComponent<PlayerController>();
            player.Die();
        }
    }
}
