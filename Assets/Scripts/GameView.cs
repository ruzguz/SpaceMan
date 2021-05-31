using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{

    // Vars for game info
    public Text coinText, scoreText, maxScoreText;
    

    // Aux vars
    PlayerController  playerController;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.sharedInstance.currentGameState  == GameState.inGame) {
            int coins = GameManager.sharedInstance.collectedObjects;
            float score = playerController.GetTravelledDistance();
            float maxScore = PlayerPrefs.GetFloat("maxscore", 0f);

            coinText.text = coins.ToString();
            scoreText.text = "Score: "+score.ToString("f1");
            maxScoreText.text = "Record: "+maxScore.ToString("f1");
        }   
    }
}
