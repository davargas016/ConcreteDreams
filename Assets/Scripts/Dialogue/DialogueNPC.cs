using UnityEngine;
using UnityEngine.UI;

public class DialogueNPC : MonoBehaviour
{
    protected bool playerInRange;

    [SerializeField] private TextAssetValue dialogueValue;
    [SerializeField] private TextAsset myDialogue;
    [SerializeField] private Notification branchingDialogueNotification;
    [SerializeField] private BoolValue unlockedFlag;
    public Signal contextOn;
    public Signal contextOff;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            contextOn.Raise();
            playerInRange = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            {
                contextOff.Raise();
                playerInRange = false;
            }
    }

    void Update()
    {
        if (!playerInRange) return;

        if (unlockedFlag != null && unlockedFlag.value == false) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogueValue.value = myDialogue;
            branchingDialogueNotification.Raise();
        }
    }
}