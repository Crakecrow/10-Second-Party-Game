using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuUI; // Reference to the main menu UI parent
    public GameObject controlsUI; // Reference to the controls UI parent

    void Start()
    {
        ShowMainMenu();
    }

    // Method to show the main menu
    public void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        controlsUI.SetActive(false);
    }

    // Method to show the controls menu
    public void ShowControls()
    {
        mainMenuUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    // Method to handle the Back to Menu button
    public void BackToMenu()
    {
        ShowMainMenu();
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quit Game action triggered!"); // Log for debugging
        Application.Quit(); // Exits the application
    }
}
