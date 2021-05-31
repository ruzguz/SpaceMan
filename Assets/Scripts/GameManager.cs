using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// values for differet game states 
public enum GameState {
    menu,
    inGame,
    gameOver
};

public class GameManager : MonoBehaviour
{

    // Game manager vars
    public int collectedObjects = 0;
    public GameState currentGameState = GameState.menu;
    public static GameManager sharedInstance;
    // Game Objects vars
    PlayerController playerController;

    void Awake() 
    {
        // Singleton 
        if (sharedInstance == null) {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("player").GetComponent<PlayerController>();
        this.StopGame();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if user start the game
        if (Input.GetButtonDown("Start") && currentGameState != GameState.inGame) {
            StartGame();
        }

        // Check if the user pause the game


        // Check if the player died
    }


    // Manage how to start the game
    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }

    // Finish the game 
    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }

    // Go to main menu
    public void BackToMenu() 
    {
        SetGameState(GameState.menu);
    }

    // Change game state
    void SetGameState(GameState newGameState)
    {
        // Show menu
        if (newGameState == GameState.menu) {
            this.StopGame();
            // Show menu
            MenuManager.sharedInstance.ShowMainMenu();

        // Resume game
        } else if (newGameState == GameState.inGame) {
            Time.timeScale = 1f;
            // Show game info
            MenuManager.sharedInstance.ShowGameInfo();
            // Set all values to start a new game
            LevelManager.sharedInstance.RemoveAllLevelBlock();
            Invoke("ReloadLevel", 0.1f);
            playerController.StartGame();
            CameraFollow.sharedInstance.ResetCameraPosition();

        // Finish the game
        } else if (newGameState == GameState.gameOver) {
            // Show Game over menu
            Time.timeScale = 0.2f;
            MenuManager.sharedInstance.ShowGameOverMenu();
        }

        // Set new game state
        this.currentGameState = newGameState;
    }


    void ReloadLevel()
    {
        LevelManager.sharedInstance.GenerateInitialBlocks();
    }


    public void CollectObject(Collectable collectable) 
    {
        collectedObjects += collectable.value;
    }

    void StopGame()
    {
        Time.timeScale = 0f;
    }


}
