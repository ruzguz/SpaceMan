using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BarType {
    healthBar, manaBar
};
public class PlayerBar : MonoBehaviour
{
    public const int DEFAULT_HEALTH = 100;
    public const int DEFAULT_MANA = 15;

    private Slider slider;
    public BarType type;

    // Start is called before the first frame update
    void Start()
    {
        slider = this.GetComponent<Slider>();

        // Assing new game values
        switch(type){
            // Values for health bar
            case BarType.healthBar:
                slider.maxValue = PlayerController.MAX_HEALTH;
                slider.minValue = PlayerController.MIN_HEALTH;
            break;

            // Values for mana bar
            case BarType.manaBar:
                slider.maxValue = PlayerController.MAX_MANA;
                slider.minValue = PlayerController.MIN_MANA;
            break;
        }
    }

    public void ResetDefaultValue() 
    {
        // Get Current value of the bar
        switch(type) {
            case BarType.healthBar:
                Debug.Log("reseteando vida");
                slider.value = DEFAULT_HEALTH;
            break;
        
            case BarType.manaBar:
                Debug.Log("Reseteando mana");
                slider.value = DEFAULT_MANA;
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get Current value of the bar
        switch(type) {
            case BarType.healthBar:
                slider.value = GameObject.Find("player").GetComponent<PlayerController>().GetHealth();
            break;
        
            case BarType.manaBar:
                slider.value = GameObject.Find("player").GetComponent<PlayerController>().GetMana();
            break;
        }
    }
}
