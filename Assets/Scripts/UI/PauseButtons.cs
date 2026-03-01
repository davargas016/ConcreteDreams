using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour
{
    public GameObject pauseMenu; 

    public void setUp()
    {
        gameObject.SetActive(true);
    }

    public void ResumeButton()
    {
        // Deactivates the pause menu when Resume is clicked
        pauseMenu.SetActive(false);
        Time.timeScale = 1; // Optional: resumes the game if you paused time
    }


    public void terminate()
    {
        Debug.Log("Game Ended");
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}