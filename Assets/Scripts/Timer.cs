using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 60f; // Set this value to the desired countdown time
    public bool timeIsRunning = false;
    public TMP_Text timeText;

    public GameObject winnerUI; // Reference to the Winner UI
    public PlayerMovement player; // Reference to the Player
    public AIChase[] enemies; // Reference to the Enemies

    // Key values for restarting and going to the main menu
    public KeyCode restartKey = KeyCode.R; // Default key for restarting the game
    public KeyCode mainMenuKey = KeyCode.M; // Default key for going to the main menu

    void Start()
    {
        timeIsRunning = true;
        winnerUI.SetActive(false); // Ensure the Winner UI is hidden at the start
    }

    void Update()
    {
        if (timeIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timeIsRunning = false;
                DisplayTime(timeRemaining);
                StopGame(); // Call method to stop the game and show the winner UI
            }
        }

        // Check for key inputs if the Winner UI is active
        if (winnerUI.activeSelf)
        {
            if (Input.GetKeyDown(restartKey))
            {
                RestartGame();
            }
            else if (Input.GetKeyDown(mainMenuKey))
            {
                GoToMainMenu();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void StopGame()
    {
        // Stop all enemy movement by setting their speed to 0
        foreach (AIChase enemy in enemies)
        {
            enemy.speed = 0;
        }

        // Disable player movement
        if (player != null)
        {
            player.enabled = false; // Assuming PlayerMovement is the script handling player movement
        }

        // Show the Winner UI
        winnerUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Debug.Log("Restart button clicked!"); // Confirm this log appears in the Console
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainGame"); // Load the specified restart scene
    }

    public void GoToMainMenu()
    {
        Debug.Log("Main Menu button clicked!"); // Confirm this log appears in the Console
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Load the specified main menu scene
    }
}
