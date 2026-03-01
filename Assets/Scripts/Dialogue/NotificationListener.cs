using UnityEngine;
using UnityEngine.Events;

public class NotificationListener : MonoBehaviour
{
    [SerializeField] private Notification myNotification;
    public UnityEvent myEvent;

    public void OnSignalRaised(){
        myEvent.Invoke();
    }

    private void OnEnable()
    {
        myNotification.RegisterListener(OnNotificationRaised);
    }

    private void OnDisable()
    {
        myNotification.UnregisterListener(OnNotificationRaised);
    }

    private void OnNotificationRaised()
    {
        // Your reaction to the notification — enable canvas, etc.
        GetComponent<BranchingDialogueController>().EnableCanvas();
    }
}