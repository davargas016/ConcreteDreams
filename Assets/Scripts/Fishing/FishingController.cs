using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishingController : MonoBehaviour
{
    private enum FishingState
    {
        Idle,
        Cast,
        WaitingForBite,
        BiteWindow,
        Resolving
    }

    [Header("Input")]
    public KeyCode castKey = KeyCode.F;
    public KeyCode cancelKey = KeyCode.Escape;

    [Header("Requirements")]
    public InventoryItem fishingRodItem;
    public InventoryItem baitItem;

    [Header("Timing (seconds)")]
    public float minBiteWait = 1.5f;
    public float maxBiteWait = 4.5f;
    public float reactionWindow = 0.75f;
    public float reelDelaySeconds = 1.0f;
    public float failDelaySeconds = 0.75f;

    [Header("References")]
    public PlayerMovement playerMovement;
    public Animator animator;
    public SpriteRenderer mainRenderer;
    public GameObject fishingVisual;
    public FishingIconDisplay iconDisplay;
    public InventoryManager inventoryManager;

    [Header("Fishing Sprites (Directional)")]
    [Tooltip("If a directional sprite is missing, it will fall back to Down if assigned.")]
    public Sprite fishingUp;
    public Sprite fishingDown;
    public Sprite fishingLeft;
    public Sprite fishingRight;

    [Header("Zone")]
    public FishingZoneInfo currentZone;
    [Tooltip("If true, will try to auto-find an active FishingZoneInfo on start.")]
    public bool autoFindZoneOnStart = true;

    [Header("Animator Params")]
    public string moveXParam = "moveX";
    public string moveYParam = "moveY";
    public string movingParam = "moving";

    // Internal
    private FishingState _state = FishingState.Idle;
    private Coroutine _routine;
    private bool _hookPressed = false;

    private void Awake()
    {
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
        if (animator == null) animator = GetComponent<Animator>();
        if (mainRenderer == null) mainRenderer = GetComponent<SpriteRenderer>();

        if (fishingVisual != null)
            fishingVisual.SetActive(false);

        if (iconDisplay != null) iconDisplay.Hide();
    }

    private void Start()
    {
        if (autoFindZoneOnStart && currentZone == null)
        {
            var zones = FindObjectsOfType<FishingZoneInfo>(true);
            foreach (var z in zones)
            {
                if (z != null && z.gameObject.activeInHierarchy)
                {
                    currentZone = z;
                    break;
                }
            }
        }
    }

    private void Update()
    {
        if (Time.timeScale <= 0f) return;

        if (ShopToggle.shopOpen) return;

        if (_state != FishingState.Idle)
        {
            if (Input.GetKeyDown(cancelKey) || MovementInputPressed())
            {
                CancelFishing();
                return;
            }
        }

        if (_state == FishingState.Idle)
        {
            if (Input.GetKeyDown(castKey))
            {
                TryStartFishing();
            }
        }
        else if (_state == FishingState.BiteWindow)
        {
            if (Input.GetKeyDown(castKey) || Input.GetMouseButtonDown(0))
            {
                _hookPressed = true;
            }
        }
        else
        {
            if ((_state == FishingState.WaitingForBite || _state == FishingState.Cast) && Input.GetKeyDown(castKey))
            {
                CancelFishing();
            }
        }
    }


    public void SetCurrentZone(FishingZoneInfo zone)
    {
        currentZone = zone;
    }

    private void TryStartFishing()
    {
        if (!CanStartFishing(out string reason))
        {
            return;
        }

        _routine = StartCoroutine(FishingRoutine());
    }

    private bool CanStartFishing(out string reason)
    {
        reason = "";

        if (currentZone == null || currentZone.waterTilemap == null)
        {
            reason = "No active zone/water tilemap.";
            return false;
        }

        if (playerMovement == null)
        {
            reason = "Missing PlayerMovement reference.";
            return false;
        }

        if (fishingRodItem != null && fishingRodItem.numberHeld <= 0)
        {
            reason = "No fishing rod.";
            return false;
        }

        if (baitItem != null && baitItem.numberHeld <= 0)
        {
            reason = "No bait.";
            return false;
        }

        Vector2Int facing = GetFacingCardinal();
        if (!IsWaterInFront(facing))
        {
            reason = "Not facing water.";
            return false;
        }

        return true;
    }

    private IEnumerator FishingRoutine()
    {
        EnterFishingVisuals();
        LockMovement(true);

        _state = FishingState.Cast;
        yield return null;

        _state = FishingState.WaitingForBite;

        float wait = Random.Range(minBiteWait, maxBiteWait);
        float t = 0f;

        while (t < wait && _state == FishingState.WaitingForBite)
        {
            t += Time.deltaTime;
            yield return null;
        }

        if (_state != FishingState.WaitingForBite)
            yield break;

        if (baitItem != null && baitItem.numberHeld > 0)
        {
            baitItem.numberHeld -= 1;
            if (baitItem.numberHeld < 0) baitItem.numberHeld = 0;
        }

        if (iconDisplay != null) iconDisplay.ShowBite();

        _state = FishingState.BiteWindow;
        _hookPressed = false;

        float w = 0f;
        while (w < reactionWindow && _state == FishingState.BiteWindow)
        {
            if (_hookPressed) break;
            w += Time.deltaTime;
            yield return null;
        }

        if (_state != FishingState.BiteWindow)
            yield break;

        _state = FishingState.Resolving;

        bool success = _hookPressed;

        if (success)
        {
            if (iconDisplay != null) iconDisplay.ShowSuccess();
            yield return new WaitForSeconds(reelDelaySeconds);
            AwardFish();
            ExitFishing();
        }
        else
        {
            if (iconDisplay != null) iconDisplay.ShowFail();
            yield return new WaitForSeconds(failDelaySeconds);
            ExitFishing();
        }
    }

    private void AwardFish()
    {
        if (currentZone == null || currentZone.lootTable == null)
            return;

        if (currentZone.lootTable.TryRoll(out InventoryItem fish, out int amount))
        {
            if (fish == null || amount <= 0) return;

            fish.numberHeld += amount;

            if (inventoryManager != null && inventoryManager.isActiveAndEnabled)
            {
                inventoryManager.ClearInventorySlots();
                inventoryManager.MakeInventorySlots();
                inventoryManager.SetTextAndButton($"+ {amount}x {fish.itemName}", false);
            }
        }
    }

    private void ExitFishing()
    {
        if (iconDisplay != null) iconDisplay.Hide();

        LockMovement(false);
        ExitFishingVisuals();

        _state = FishingState.Idle;
        _routine = null;
    }

    private void CancelFishing()
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
            _routine = null;
        }

        if (iconDisplay != null) iconDisplay.Hide();

        LockMovement(false);
        ExitFishingVisuals();

        _state = FishingState.Idle;
    }

    private void LockMovement(bool locked)
    {
        if (playerMovement != null)
            playerMovement.canMove = !locked;

        if (animator != null && !string.IsNullOrEmpty(movingParam))
            animator.SetBool(movingParam, false);
    }

    private void EnterFishingVisuals()
    {
        if (mainRenderer != null) mainRenderer.enabled = false;

        if (fishingVisual != null)
        {
            fishingVisual.SetActive(true);

            var sr = fishingVisual.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Vector2Int facing = GetFacingCardinal();
                sr.sprite = GetFishingSpriteForFacing(facing);
            }
        }
    }

    private Sprite GetFishingSpriteForFacing(Vector2Int facing)
    {
        Sprite fallback = fishingDown != null ? fishingDown : fishingRight != null ? fishingRight :
                          fishingLeft != null ? fishingLeft : fishingUp;

        if (facing == Vector2Int.up)    return fishingUp != null ? fishingUp : fallback;
        if (facing == Vector2Int.down)  return fishingDown != null ? fishingDown : fallback;
        if (facing == Vector2Int.left)  return fishingLeft != null ? fishingLeft : fallback;
        if (facing == Vector2Int.right) return fishingRight != null ? fishingRight : fallback;

        return fallback;
    }

    private void ExitFishingVisuals()
    {
        if (fishingVisual != null) fishingVisual.SetActive(false);
        if (mainRenderer != null) mainRenderer.enabled = true;
    }

    private bool MovementInputPressed()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        return Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f;
    }

    private Vector2Int GetFacingCardinal()
    {
        if (animator == null)
            return Vector2Int.down;

        float mx = animator.GetFloat(moveXParam);
        float my = animator.GetFloat(moveYParam);

        if (Mathf.Abs(mx) < 0.01f && Mathf.Abs(my) < 0.01f)
            return Vector2Int.down;

        if (Mathf.Abs(mx) >= Mathf.Abs(my))
            return mx >= 0 ? Vector2Int.right : Vector2Int.left;

        return my >= 0 ? Vector2Int.up : Vector2Int.down;
    }

    private bool IsWaterInFront(Vector2Int facing)
    {
        if (currentZone == null || currentZone.waterTilemap == null) return false;

        Tilemap water = currentZone.waterTilemap;

        Vector3Int playerCell = water.WorldToCell(transform.position);
        Vector3Int targetCell = playerCell + new Vector3Int(facing.x, facing.y, 0);

        return water.HasTile(targetCell);
    }
}
