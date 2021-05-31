using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public Canvas menuCanvas;
    public Canvas gameOverCanvas;
    public Canvas gameCanvas;

    public static MenuManager sharedInstance;

    void Awake() 
    {
        if (sharedInstance == null) {
            sharedInstance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMainMenu() 
    {
        menuCanvas.enabled = true;
        // hide other menus
        this.HideGameInfo();
        this.HideGameOverMenu();
    }

    public void HideMainMenu() 
    {
        menuCanvas.enabled = false;
    }

    public void ShowGameOverMenu() 
    {
        GameOverView.sharedInstance.UpdateInfo();
        gameOverCanvas.enabled = true;
        // hide other menus
        this.HideMainMenu();
        this.HideGameInfo();
    }

    public void HideGameOverMenu() 
    {
        gameOverCanvas.enabled = false;
    }

    public void ShowGameInfo() 
    {
        gameCanvas.enabled = true;
        // hide other menus
        this.HideMainMenu();
        this.HideGameOverMenu();
    }

    public void HideGameInfo()
    {
        gameCanvas.enabled = false;
    }

    public void ExitGame() {
        Application.Quit();
    }
}
