using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Values for different collectables
public enum CollectableType {
    healthPotion,
    manaPotion,
    money
};


public class Collectable : MonoBehaviour
{



    // Components
    SpriteRenderer sprite;
    CircleCollider2D itemCollider;

    //Collectable vars
    public CollectableType type = CollectableType.money;
    [SerializeField]
    bool hasBeenCollected = false;
    public int value = 1;
    GameObject player;


    void Awake() 
    {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();
    }


    void Start() 
    {
        player = GameObject.Find("player");
    }


    // When the player collect something
    void OnTriggerEnter2D(Collider2D c) 
    {
        if (c.tag == "Player") {
            Collect();
        }
    }

    // Enable collectable
    public void Show()
    {
        sprite.enabled = true;
        itemCollider.enabled = true;
        this.hasBeenCollected = false;
    }


    // Disable Collectable
    public void Hide() 
    {
        sprite.enabled = false;
        itemCollider.enabled = false;
    }


    // Collect
    public void Collect() 
    {
        Hide();
        this.hasBeenCollected = true;
        switch(this.type) {
            // When collect a health postion
            case CollectableType.healthPotion:
                player.GetComponent<PlayerController>().CollectHealth(this.value);
            break;

            // When Collect a mana potion
            case CollectableType.manaPotion:
                player.GetComponent<PlayerController>().CollectMana(this.value);
            break;

            // When Collect money
            case CollectableType.money:
                GetComponent<AudioSource>().Play();
                GameManager.sharedInstance.CollectObject(this);
            break;
        }
    }
}
