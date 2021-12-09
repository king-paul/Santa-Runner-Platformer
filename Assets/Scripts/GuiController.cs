using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuiController : MonoBehaviour
{
    public GameObject HUD;
    //public TextMeshProUGUI distanceText;
    //public TextMeshProUGUI gameOverDistanceText;
    public TextMeshProUGUI presentsCount;
    //public RectTransform jumpMeter;
    public RectTransform staminaMeter;
    public GameObject titleScreen;
    public GameObject gameOverUI;
    public Button continueButton;

    private TextMeshProUGUI[] continueText;

    private float maxBarHeight = 0;
    private float maxBarWidth;

    // Start is called before the first frame update
    void Start()
    {
        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
            HUD.SetActive(false);
        }
        gameOverUI.SetActive(false);

        maxBarWidth = staminaMeter.rect.width;
    }

    /// <summary>
    /// Updates the vertical jump meter on the gui to match the player's jump force.
    /// </summary>
    /// <param name="jumpForce">Sets the current value to set the bar at</param>
    /// <param name="maxJumpForce">Sets tge maximum value that the jump meter can be set at</param>
    //public void SetJumpMeter(float jumpForce, float maxJumpForce)
    //{
    //    float newHeight = maxBarHeight / maxJumpForce * jumpForce;
    //    jumpMeter.sizeDelta = new Vector2(jumpMeter.rect.width, newHeight);
    //}

    public void SetStaminaMeter(float curStamina, float maxStamina)
    {
        float width = maxBarWidth / maxStamina * curStamina;
        staminaMeter.sizeDelta = new Vector2(width, staminaMeter.rect.height);
    }

    /// <summary>
    /// Displays the Game over Menu on the screen with 3 buttons
    /// </summary>
    /// <param name="currentCoins">The coins that the player currently has</param>
    /// <param name="coinsNeeded">The number of coins needed to click the continue button</param>
    //public void ShowGameOverScreen(int currentCoins, int coinsNeeded)
    //{
    //    gameOverUI.SetActive(true);
    //    continueText = continueButton.GetComponentsInChildren<TextMeshProUGUI>();
    //    continueText[1].text = coinsNeeded.ToString();

    //    if (currentCoins >= coinsNeeded)
    //    {
    //        continueButton.interactable = true;            

    //        foreach(TextMeshProUGUI text in continueText)
    //        {
    //            text.color = Color.yellow;
    //        }            
    //    }
    //    else
    //    {
    //        continueButton.interactable = false;

    //        foreach (TextMeshProUGUI text in continueText)
    //        {
    //            text.color = new Color(127, 127, 0);
    //        }
    //    }

    //}

}
