using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
 
    // Text Vars
    public Text coinText, scoreText, maxScoreText;
 
    PlayerController player;

    public static GameOverView sharedInstance;

    // Start is called before the first frame update
    void Start()
    {
        sharedInstance = (sharedInstance == null)?this:null;
        player = GameObject.Find("player").GetComponent<PlayerController>();
    }

    public void UpdateInfo()
    {
        int coins = GameManager.sharedInstance.collectedObjects;
        float score = player.GetTravelledDistance();
        float maxScore = PlayerPrefs.GetFloat("maxscore");

        coinText.text = "Coins: "+coins.ToString();
        scoreText.text = "Score: "+score.ToString("f1");
        maxScoreText.text = "Record: "+ maxScore.ToString("f1");
    }
}
