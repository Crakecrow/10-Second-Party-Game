using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Ensure we include this for scene management

public class Player_Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;

    public GameObject gameOverUI; // Reference to the Game Over UI
    public Button restartButton; // Reference to the Restart button
    public Button mainMenuButton; // Reference to the Main Menu button

    public KeyCode restartKey = KeyCode.R; // Key for restarting the game
    public KeyCode mainMenuKey = KeyCode.M; // Key for going to the main menu

    public ParticleSystem explosionEffect; // Particle system for explosion effect
    public AudioClip explosionSound; // Sound effect for explosion

    private AudioSource audioSource;

    void Start()
    {
        maxHealth = health;
        gameOverUI.SetActive(false); // Ensure the Game Over UI is hidden at the start
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        // Check for key inputs if the Game Over UI is active
        if (gameOverUI.activeSelf)
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

    // Method to take damage
    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        // Check if the player is dead
        if (health <= 0)
        {
            PlayerDied();
        }
    }

    // Method to handle player death
    void PlayerDied()
    {
        Debug.Log("Player is dead!"); // Confirm this log appears in the Console

        // Hide the player GameObject
        gameObject.SetActive(false);

        // Instantiate and play the explosion effect
        if (explosionEffect != null)
        {
            ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            explosion.Play();

            // Destroy the explosion effect after it finishes playing
            Destroy(explosion.gameObject, explosion.main.duration);
            Debug.Log("Explosion effect played."); // Debug log
        }
        else
        {
            Debug.LogWarning("Explosion effect is not assigned."); // Debug log
        }

        // Play the explosion sound effect
        if (explosionSound != null)
        {
            Debug.Log("Playing explosion sound."); // Debug log
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            Debug.Log("Explosion sound played."); // Debug log
        }
        else
        {
            Debug.LogWarning("Explosion sound is not assigned."); // Debug log
        }

        gameOverUI.SetActive(true); // Show Game Over UI
        GameManager.instance.TriggerLose(); // Trigger the lose sound and logic
    }

    // Method for restarting the game
    public void RestartGame()
    {
        Debug.Log("Restart action triggered!"); // Confirm this log appears in the Console
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Load the specified restart scene
    }

    // Method for going to the main menu
    public void GoToMainMenu()
    {
        Debug.Log("Main Menu action triggered!"); // Confirm this log appears in the Console
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Load the specified main menu scene
    }
}
