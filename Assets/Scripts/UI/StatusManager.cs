using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public float currentHunger = 100f;
    public float currentThirst = 100f;
    public float maxHunger = 100f;
    public float maxThirst = 100f;

    public float hungerDrainRate = 1f;
    public float thirstDrainRate = 2f;

    public HealthManager healthManager;

    private bool isStarving = false;

    void Update()
    {
        currentHunger -= hungerDrainRate * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);

        currentThirst -= thirstDrainRate * Time.deltaTime;
        currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);

        HandleStarvation();
    }

    private void HandleStarvation()
    {
        if (currentHunger <= 0f)
        {
            if (!isStarving)
            {
                isStarving = true;
                Debug.Log("Player is starving! Starting health drain.");

                InvokeRepeating(nameof(HealthDrain), 5f, 5f);
            }
        }

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

    }

    public void HealthDrain()
    {
        if (healthManager == null) return;
        if (healthManager.IsDead) return;

        healthManager.TakeDamage(1f);

        Debug.Log($"Starvation damage! Health is now {healthManager.playerHealth}");

        if (healthManager.IsDead)
        {
            CancelInvoke(nameof(HealthDrain));
            isStarving = false;
        }
    }
}
