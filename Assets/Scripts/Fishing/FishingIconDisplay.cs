using UnityEngine;

public class FishingIconDisplay : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] private SpriteRenderer iconRenderer;

    [Header("Sprites")]
    public Sprite biteSprite;     // "!"
    public Sprite successSprite;  // "check"
    public Sprite failSprite;     // "X"

    private void Awake()
    {
        if (iconRenderer == null)
            iconRenderer = GetComponentInChildren<SpriteRenderer>();

        Hide();
    }

    public void ShowBite()
    {
        if (iconRenderer == null) return;
        iconRenderer.sprite = biteSprite;
        iconRenderer.enabled = true;
    }

    public void ShowSuccess()
    {
        if (iconRenderer == null) return;
        iconRenderer.sprite = successSprite;
        iconRenderer.enabled = true;
    }

    public void ShowFail()
    {
        if (iconRenderer == null) return;
        iconRenderer.sprite = failSprite;
        iconRenderer.enabled = true;
    }

    public void Hide()
    {
        if (iconRenderer == null) return;
        iconRenderer.enabled = false;
        iconRenderer.sprite = null;
    }
}
