using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Enemy vars
    Rigidbody2D enemyRigidbody;
    SpriteRenderer enemySprite;
    public float enemySpeed = 3f;
    public int enemyDamage = 15;
    public bool facingRight = true;
    Vector3 startPosition;
    GameObject enemyParent;

    void Awake() 
    {
        enemyRigidbody = this.GetComponent<Rigidbody2D>();
        enemySprite = this.GetComponent<SpriteRenderer>();
        //startPosition = this.transform.position;

    }

    // Start is called before the first frame update
    void Start()
    {
        //this.transform.position = startPosition;
        enemyParent = (this.transform.parent.gameObject != null)?this.transform.parent.gameObject:null;
    }

    // is called in a fixed rate
    void FixedUpdate() {    



        // Enemy Movement
        float currentEnemySpeed = enemySpeed;

        if (facingRight) {
            currentEnemySpeed = enemySpeed;
            enemySprite.flipX = true;
        } else {
            currentEnemySpeed = -enemySpeed;
            enemySprite.flipX = false;
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame) {
            enemyRigidbody.velocity = new Vector2(currentEnemySpeed, 0);
        }

        // enemy don't get out of the platform attached (if have a parent)
        if (enemyParent != null) {
            // getting enemy and parent width
            float parentWidth = enemyParent.GetComponent<SpriteRenderer>().size.x;
            float enemyWidth = enemySprite.size.x;

            // Calculating bounds
            float leftLimit = enemyParent.transform.position.x - parentWidth/2;
            float rightLimit = enemyParent.transform.position.x + parentWidth/2; 

            // enemy is in the right limit 
            if ((this.transform.position.x + enemyWidth/2) >= rightLimit) {
                facingRight = false;
            }

            if ((this.transform.position.x - enemyWidth/2) <= leftLimit) {
                facingRight = true;
            } 
            
        }

    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().CollectHealth(-enemyDamage);
        }
    }
}
