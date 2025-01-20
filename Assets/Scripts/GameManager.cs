using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Ensure we include this for scene management

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI objectiveText;
    public GameObject winnerScreen; // Reference to the winner screen GameObject
    public int totalShards = 3;
    private int collectedShards = 0;
    private bool gameWon = false;

    public AudioClip winSound; // Sound for winning
    public AudioClip loseSound; // Sound for losing
    public AudioClip announcerWinSound; // Announcer sound for winning
    public AudioClip announcerLoseSound; // Announcer sound for losing
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateObjectiveText();
        winnerScreen.SetActive(false); // Initially hide the winner screen
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (gameWon)
        {
            // Listen for key presses to reset or go to main menu
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetGame();
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                GoToMainMenu();
            }
        }
    }

    public void CollectShard()
    {
        if (gameWon)
        {
            return;
        }

        collectedShards++;
        UpdateObjectiveText();

        if (collectedShards >= totalShards)
        {
            gameWon = true;
            StartCoroutine(HandleWin());
        }
    }

    void UpdateObjectiveText()
    {
        objectiveText.text = "Objective: Collect Shards " + collectedShards + "/" + totalShards;
    }

    private IEnumerator HandleWin()
    {
        // Play win sound
        if (winSound != null)
        {
            audioSource.PlayOneShot(winSound);
            Debug.Log("Win sound played.");
        }
        else
        {
            Debug.LogWarning("Win sound is not assigned.");
        }

        // Play announcer win sound
        if (announcerWinSound != null)
        {
            yield return new WaitForSecondsRealtime(winSound.length);
            audioSource.PlayOneShot(announcerWinSound);
            Debug.Log("Announcer win sound played.");
        }
        else
        {
            Debug.LogWarning("Announcer win sound is not assigned.");
        }

        // Display winner screen
        winnerScreen.SetActive(true);

        // Stop all movement
        Time.timeScale = 0f;

        // Wait for a short moment to allow UI to display
        yield return new WaitForSecondsRealtime(1f);

        Debug.Log("Objective Complete! Game Won!");
    }

    public void TriggerLose()
    {
        StartCoroutine(HandleLose());
    }

    private IEnumerator HandleLose()
    {
        // Play lose sound
        if (loseSound != null)
        {
            audioSource.PlayOneShot(loseSound);
            Debug.Log("Lose sound played.");
        }
        else
        {
            Debug.LogWarning("Lose sound is not assigned.");
        }

        // Play announcer lose sound
        if (announcerLoseSound != null)
        {
            yield return new WaitForSecondsRealtime(loseSound.length);
            audioSource.PlayOneShot(announcerLoseSound);
            Debug.Log("Announcer lose sound played.");
        }
        else
        {
            Debug.LogWarning("Announcer lose sound is not assigned.");
        }

        gameWon = true; // Prevent further actions
        Time.timeScale = 0f; // Pause the game

        // Display game over UI or handle lose logic here
    }

    private void ResetGame()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene (replace "MainMenu" with your scene name)
    }
}
