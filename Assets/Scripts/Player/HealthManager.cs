using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public float playerHealth;
    public float maxHealth = 10f;

    private bool isDead = false;
    [SerializeField] private GameObject gameOverUI;

    void Start()
    {
        playerHealth = maxHealth;
    }

    // Call this whenever something changes health
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        playerHealth -= amount;
        playerHealth = Mathf.Clamp(playerHealth, 0f, maxHealth);

        if (playerHealth <= 0f)
        {
            Die();
        }
    }

    public void RestoreHealth(InventoryItem item)
    {
        if (isDead) return;
        if (item == null) return;

        playerHealth += item.healthRestore;
        playerHealth = Mathf.Clamp(playerHealth, 0f, maxHealth);
        Debug.Log("Player Health Restored!");
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
    }
    public bool IsDead => isDead;
}