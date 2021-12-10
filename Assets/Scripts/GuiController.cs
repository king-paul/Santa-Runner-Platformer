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
    public RectTransform staminaMeter;
    public GameObject titleScreen;
    public GameObject gameOverUI;

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

}
