using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public float currentHunger = 100f;
    public float currentThirst = 100f;
    public float maxHunger = 100f;
    public float maxThirst = 100f;

    public float hungerDrainRate = 1f; // Hunger lost per second
    public float thirstDrainRate = 2f; // Thirst lost per second

    public HealthManager healthManager;

    // Controls whether we already started the starvation drain loop
    private bool isStarving = false;

    void Update()
    {
        // Drain hunger/thirst over time
        currentHunger -= hungerDrainRate * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);

        currentThirst -= thirstDrainRate * Time.deltaTime;
        currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);

        HandleStarvation();
    }

    private void HandleStarvation()
    {
        // Start starvation drain ONCE when hunger reaches 0
        if (currentHunger <= 0f)
        {
            if (!isStarving)
            {
                isStarving = true;
                Debug.Log("Player is starving! Starting health drain.");

                // After 5 seconds, drain once, then repeat every 5 seconds
                InvokeRepeating(nameof(HealthDrain), 5f, 5f);
            }
        }
        // Stop starvation drain when hunger is restored above 0
        else
        {
            if (isStarving)
            {
                isStarving = false;
                Debug.Log("Player is no longer starving. Stopping health drain.");

                CancelInvoke(nameof(HealthDrain));
            }
        }
    }

    public void ConsumeFood(InventoryItem item)
    {
        if (item == null) return;

        currentHunger += item.hungerAmount;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);

        currentThirst += item.thirstAmount;
        currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);

        Debug.Log($"Consumed {item.itemName}. Hunger and Thirst restored!");
        // HandleStarvation() will stop drain on next Update automatically,
        // but you could call it here too if you want instant stop:
        // HandleStarvation();
    }

    public void HealthDrain()
    {
        if (healthManager == null) return;
        if (healthManager.IsDead) return;

        healthManager.TakeDamage(1f);

        Debug.Log($"Starvation damage! Health is now {healthManager.playerHealth}");

        // If dead, stop the repeating invoke
        if (healthManager.IsDead)
        {
            CancelInvoke(nameof(HealthDrain));
            isStarving = false;
        }
    }
}
