using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    // Method to load the MainGame scene
    public void LoadMainGameScene()
    {
        Debug.Log("Button clicked! Loading MainGame scene...");
        SceneManager.LoadScene("MainGame"); // Ensure the scene name matches exactly
    }
}
