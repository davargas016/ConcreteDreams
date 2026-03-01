using UnityEngine;

public class GameEndZone : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject gameOverScreen;   
    public MonoBehaviour playerMovementScript; 
    public bool freezeTime = true;

    private bool triggered = false;

    private void Awake()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (!other.CompareTag("Player")) return;

        triggered = true;

        // Disable player movement
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        // Show Game Over UI
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        // Freeze the game
        if (freezeTime)
            Time.timeScale = 0f;
    }
}