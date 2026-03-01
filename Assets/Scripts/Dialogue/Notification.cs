using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Notification")]
public class Notification : ScriptableObject
{
    private List<System.Action> listeners = new List<System.Action>();

    public void Raise()
    {
        foreach (var listener in listeners)
        {
            listener?.Invoke();
        }
    }

    public void RegisterListener(System.Action listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(System.Action listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}