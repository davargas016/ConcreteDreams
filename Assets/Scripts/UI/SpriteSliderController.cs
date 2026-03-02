using UnityEngine;
using UnityEngine.UI;

public class SpriteSliderController : MonoBehaviour
{
    public enum TrackedStat { Hunger, Thirst, Health}

    [Header("Status Source")]
    [SerializeField] private StatusManager statusManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private TrackedStat trackedStat = TrackedStat.Hunger;

    [Header("Optional UI (will be synced)")]
    [SerializeField] private Slider mySlider;
    [SerializeField] private Image targetImage;

    [Tooltip("Order: [Full (100-91), 90s, 80s, 70s, 60s, 50s, 40s, 30s, 20s, 10s, Empty (0)]")]
    [SerializeField] private Sprite[] stateSprites = new Sprite[11];

    [Header("Update")]
    [SerializeField] private bool updateEveryFrame = true;

    void Awake()
    {
        if (!targetImage) targetImage = GetComponent<Image>();
    }

    void OnEnable()
    {
        Refresh();
    }

    void Update()
    {
        if (updateEveryFrame)
            Refresh();
    }

    public void Refresh()
    {
        if (!statusManager || !targetImage || stateSprites == null || stateSprites.Length < 11)
        {
            Debug.LogWarning("SpriteSliderController: Missing refs or sprites (need 11).");
            return;
        }

        float current, max;

        switch (trackedStat)
        {
            case TrackedStat.Hunger:
                current = statusManager.currentHunger;
                max = statusManager.maxHunger;
                break;
            case TrackedStat.Thirst:
                current = statusManager.currentThirst;
                max = statusManager.maxThirst;
                break;
            case TrackedStat.Health:
                current = healthManager.playerHealth;
                max = healthManager.maxHealth;
                break;

            default:
                Debug.Log("Issue Assigning Status");
                return;
        }

        if (max <= 0f) max = 1f;

        if (mySlider)
        {
            if (mySlider.maxValue != max) mySlider.maxValue = max;
            if (!Mathf.Approximately(mySlider.value, current)) mySlider.value = current;
        }

        float percent = Mathf.Clamp01(current / max) * 100f;

        int idx = IndexForPercent(percent);
        targetImage.sprite = stateSprites[idx];
    }

    private int IndexForPercent(float percent)
    {
        if (percent <= 0f) return 10;     // exact empty
        if (percent >= 91f) return 0;     // full band
        int band = Mathf.FloorToInt((90f - percent) / 10f) + 1;
        return Mathf.Clamp(band, 1, 9);
    }
}