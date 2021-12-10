// Author: Paul King

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public enum GameState
{
    Idle,
    Running,
    StageComplete,
    Dead
}

public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance;
    private AudioSource m_MusicSource;

    [Header("Game Objects")]
    public Transform m_StartingPoint;

    [Header("Variables")]
    [SerializeField] private float m_Gravity = -9.8f;
    //[SerializeField] int continueCost = 10;
    //public Vector3 m_LastCheckpointPos;    

    [Header("Music")]
    public AudioClip m_TitleMusic;
    public AudioClip m_RunningMusic;

    private int m_Presents;
    private bool m_GameRunning;
    [SerializeField]
    private GameState m_State;
    private GameObject m_Player;
    private PlayerController playerController;
    private GuiController gui;

    // properties
    public GameState State { get => m_State; }
    /// <summary>
    /// Returns the gravity being used in the scene (read only)
    /// </summary>
    public float Gravity { get => m_Gravity; }

    /// <summary>
    /// Returns the current game state to check whether or not the game is
    /// still running (read only)
    /// </summary>
    public bool GameRunning { get => m_GameRunning; }

    /// <summary>
    /// Used to access the last checkpoint position variable
    /// </summary>
    public Vector3 LastCheckpointPos
    {
        //get => m_LastCheckpointPos;
        set => LastCheckpointPos = value;
    }    

    /***********************
     * functions / methods *
     ***********************/     
    /// <summary>
    /// Changes the game state to the running state
    /// </summary>
    public void StartGame() { UpdateGameState(GameState.Running); }
    /// <summary>
    /// Changes the game state to the dead state
    /// </summary>
    public void EndGame() { UpdateGameState(GameState.Dead); }
    /// <summary>
    /// Moves the player gamer object back to the start position
    /// and changes the game state to idel
    /// </summary>
    public void RestartGame()
    {
        // set the last checkpoint to the start of the level
        //m_LastCheckpointPos = new Vector3(m_StartingPoint.position.x,
            //m_StartingPoint.position.y, 0);

        //UpdateGameState(GameState.Idle);
    }

    /// <summary>
    /// Reloads the unity scene by loading the first one in the project build
    /// </summary>
    public void RestartScene() { SceneManager.LoadScene("Main Scene"); }
    public void QuitGame() {
        Debug.Log("Quit Button Clicked");
        Application.Quit(); 
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    /// <summary>
    /// Increases the number of coins collected by 1
    /// </summary>
    public void AddPresent() { 
        m_Presents++;
        gui.presentsCount.text = m_Presents.ToString();
    }

    void Awake()
    {
        m_Instance = this;
    }

    void Start()
    {
        // Find gameobjects
        //try
        //{
            m_Player = GameObject.FindWithTag("Player");
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            m_MusicSource = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
            gui = GameObject.Find("Canvas").GetComponent<GuiController>();

            //m_LastCheckpointPos = transform.position;

            // Set ui values
            //gui.SetJumpMeter(0, 0);

            // Set game state
            //UpdateGameState(GameState.Idle);
            m_GameRunning = true;
            m_Presents = 0;

            // Set music to title music
            if (m_MusicSource != null && m_TitleMusic != null)
            {
                m_MusicSource.clip = m_TitleMusic;
                m_MusicSource.Play();
            }
        

        //}
        //catch (NullReferenceException e)
        //{
        //    Debug.LogError("Exception on GameManager: " + e.Message);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // Running
        if (m_State == GameState.Running)
        {
            //gui.distanceText.text = ((int)m_Player.transform.position.x).ToString();
            //gui.coinText.text = m_coins.ToString();
        }

        // Quit the game when ESC is pressed
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("You chose to quit the application");
            Application.Quit();
        }
       
    }

    /// <summary>
    /// Sets the current state of the game. Can be Idle, Running, StageComplete or Dead
    /// </summary>
    /// <param name="_newState">Takes GameState enumerator as a parameter</param>
    public void UpdateGameState(GameState _newState)
    {
        //Debug.Log("New game state: " + _newState);
        m_State = _newState;

        switch (_newState)
        {
            // Idle State
            case GameState.Idle:                
                //gui.titleScreen.SetActive(true);
                //m_Player.transform.position = m_LastCheckpointPos;
                //gui.gameOverUI.SetActive(false);                
            break;

            // Running State
            case GameState.Running:
                //m_Player.transform.position = m_LastCheckpointPos;
                m_Player.GetComponent<CharacterController>().enabled = true;

                //gui.titleScreen.SetActive(false);

                //gui.titleScreen.SetActive(false);
                //gui.gameOverUI.SetActive(false);
                //gui.HUD.SetActive(true);

                m_MusicSource.clip = m_RunningMusic;
                m_MusicSource.Play();

                //playerController.onBegin.Invoke();
            break;

            // Dead state
            case GameState.Dead:
                gui.gameOverUI.SetActive(true);

                //Debug.Log("Last Checkpoint" + m_LastCheckpointPos);

                //m_MusicSource.Stop();

                //gui.HUD.SetActive(false);
                //gui.ShowGameOverScreen(m_coins, continueCost);

                //gui.gameOverDistanceText.text = (int)m_Player.transform.position.x + " feet";

                //Globals.coins = m_coins;
                //PlayerPrefs.SetInt("coins", m_coins);
            break;

            default:                
                break;
        }

    }

    /// <summary>
    /// Updates the vertical jump meter on the gui to match the player's jump force
    /// </summary>
    /// <param name="jumpForce">the jump force applied</param>
    /// <param name="maxJumpForce">the maximim jump force that the player can apply</param>
    //public void SetJumpMeter(float jumpForce, float maxJumpForce)
    //{
        //gui.SetJumpMeter(jumpForce, maxJumpForce);
    //}

    public void SetStaminaMeter(float curStamina, float maxStamina)
    {
        gui.SetStaminaMeter(curStamina, maxStamina);
    }

    /// <summary>
    /// Teleports the player back to the last checkpoint passed and
    /// changes the game state to Running
    /// </summary>
    //public void ContinueFromCheckpoint()
    //{
    //    if(m_coins >= continueCost)
    //    {
    //        m_coins -= continueCost;
    //        Globals.coins = m_coins;
    //        UpdateGameState(GameState.Running);
    //    }
    //}

}
