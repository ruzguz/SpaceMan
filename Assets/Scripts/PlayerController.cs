using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    public static PlayerController sharedInstance;

    [SerializeField]
    Vector3 startPosition;
    [SerializeField]
     float jumpForce = 10f;
     [SerializeField]
     float runningSpeed = 5f;
     SpriteRenderer playerSR;
     Animator animator;
     Rigidbody2D playerRigidBody;
     [SerializeField]
     LayerMask groundMask;
     float jumpAux = 1.5f;
     float jumpAuxOffset = 0.25f;
     float maxTravelledDistance = 0;

     // Super jump vars

     public const int SUPERJUMP_COST = 5;
     public const float SUPERJUMP_FORCE = 1.5f;



     // Consts for change animation states
     private const string STATE_ALIVE = "isAlive";
     private const string STATE_IS_ON_THE_GROUND = "isTouchingTheGround";
     private const string STATE_FALLING = "IsFalling";
     private const string STATE_RUNNING = "isRunning";

     // Boundaries vars
     Vector2 screenBounds;
     float objectWidth;

     // Collectable vars
     public int healthPoints, manaPoints;

     public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15, 
                      MAX_HEALTH = 200, MAX_MANA = 30,
                      MIN_HEALTH = 0, MIN_MANA = 0;

    void Awake() {
        sharedInstance = this;
        playerRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundMask = LayerMask.GetMask("Ground");
        playerSR = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set animation values 
        this.DefaultAnimationValues();
        // Set Start Game Values
        startPosition = this.transform.position;
        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;
        
        // Initalize bounds
        screenBounds = Camera.main.ScreenToWorldPoint(
            new Vector3(
                Screen.width,
                Screen.height,
                Camera.main.transform.position.z
            )
        );

        // side bounds
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x / 2;



    }

    // Set values te start new game 
    public void StartGame()
    {
        // Set animation values
        this.DefaultAnimationValues();
        // Set pos & velocity
        this.transform.position = startPosition;
        playerRigidBody.velocity = Vector2.zero;
        this.healthPoints = PlayerBar.DEFAULT_HEALTH;
        this.manaPoints = PlayerBar.DEFAULT_MANA;

    }

    // Update is called once per frame
    void Update()
    {

        animator.SetBool(STATE_IS_ON_THE_GROUND, IsTouchingTheGround());
        animator.SetBool(STATE_FALLING, IsFalling());

        // DEBUG MSG:
        Debug.DrawRay(transform.position, Vector2.down*jumpAux, Color.red);
    }

    void FixedUpdate() 
    {

        // Check if the layer wanna make a super jump
        if (Input.GetButtonDown("Superjump")){
            Jump(true);
        }

        // Check if the player press jump buttom
        if (Input.GetButtonDown("Jump")) {
            Jump(false);
        }

        // Running handler 
        if (Input.GetAxis("Horizontal") != 0) {
            animator.SetBool(STATE_RUNNING, true);
            Run();
        } else {
            animator.SetBool(STATE_RUNNING, false);
            playerRigidBody.velocity = new Vector2(0, playerRigidBody.velocity.y);
        }
    }

    // is called after each frame 
    void LateUpdate() {
        
        // Check if the player is within the horizontal limits
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        transform.position = viewPos;
    }

    // Set default animations values
    void DefaultAnimationValues()
    {
        animator.SetBool(STATE_ALIVE, true);             // Character is alive
        animator.SetBool(STATE_IS_ON_THE_GROUND, false); // Character is not in the ground
        animator.SetBool(STATE_FALLING, true);           // Character is falling
        animator.SetBool(STATE_RUNNING, false);          // Character is not running
        animator.Play("Land");
    }

    // Funtion to control the Player Jumping 
    void Jump(bool superJump) 
    {

        if (IsTouchingTheGround()) {

            GetComponent<AudioSource>().Play();

            float jumpForceFactor = jumpForce;

            // check if player can make a super jump
            if(superJump && manaPoints >= SUPERJUMP_COST) {
                manaPoints -= SUPERJUMP_COST;
                jumpForceFactor *= SUPERJUMP_FORCE;
            }
        
            playerRigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
        } 
    }

    // Funtion to control the Player Running 
    void Run() 
    {
        // Calculate Player dir
        float dir = (Input.GetAxis("Horizontal") < 0)?-runningSpeed:runningSpeed;
        
        // Reder sprite in player dir
        playerSR.flipX = (dir < 0)?true:false;

        // Move player
        playerRigidBody.velocity = new Vector2(dir, playerRigidBody.velocity.y);
        

    }

    // Method to check if the player is touching the ground
    bool IsTouchingTheGround()
    {
        CapsuleCollider2D playerCollider = GetComponent<CapsuleCollider2D>();
        
        
        // If player is on the ground range and is not jumping
        
        // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, jumpAux, groundMask);
        
        // RaycastHit2D hit = Physics2D.CapsuleCast(playerCollider.bounds.center, playerCollider.bounds.size, 
        //                                         CapsuleDirection2D.Vertical, 0, 
        //                                         Vector2.down, jumpAux, groundMask);

        RaycastHit2D hit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size,0,
                                            Vector2.down, jumpAux, groundMask);

        // Check if the player is avobe of the platform
        bool playerIsAbove = false;
        if (hit && hit.collider.tag == "Ground") {
            Transform groundPosition = hit.collider.GetComponent<Transform>();
            float diff = this.transform.position.y - groundPosition.position.y;
            // if player position - ground position >= jump range them the player is above of the platform 
            if (diff >= jumpAux - jumpAuxOffset) {
                playerIsAbove = true;
            }
        }

        // if player is in jump range, is above of the platform and is falling them is toouching the ground
        if (hit && playerIsAbove && playerRigidBody.velocity.y <= 0.5) {
            return true;
        } else {
            return false;
        }
        
    }

    bool IsFalling()
    {
        if (!IsTouchingTheGround() && playerRigidBody.velocity.y < 0){
            return true;
        } else {
            return false;
        }
    }

    public void Die() {

        // Checking for new record
        float travelledDistance = this.GetTravelledDistance();
        float previusMaxDistance = PlayerPrefs.GetFloat("maxscore", 0f);

        if (travelledDistance > previusMaxDistance) {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }

        // Setting values for game over
        animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if (this.healthPoints > MAX_HEALTH) this.healthPoints = MAX_HEALTH;

        if (this.healthPoints <= 0) this.Die();
    }

    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if (this.manaPoints > MAX_MANA) this.manaPoints = MAX_MANA;
    }

    public int GetHealth()
    {
        return this.healthPoints;
    }

    public int GetMana()
    {
        return this.manaPoints;
    }


    public float GetTravelledDistance()
    {
        float currentDistance = this.transform.position.y;

        if (currentDistance > maxTravelledDistance) {
            maxTravelledDistance = currentDistance;
        }

        return maxTravelledDistance;

    }

}
