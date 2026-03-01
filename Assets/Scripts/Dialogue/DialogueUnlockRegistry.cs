using System.Collections.Generic;
using UnityEngine;

public class DialogueUnlockRegistry : MonoBehaviour
{
    [System.Serializable]
    public class UnlockEntry
    {
        public string key;     // e.g. "npc3"
        public BoolValue flag; // e.g. NPC3_Unlocked
    }

    [SerializeField] private List<UnlockEntry> entries = new List<UnlockEntry>();

    private Dictionary<string, BoolValue> map;

    void Awake()
    {
        map = new Dictionary<string, BoolValue>();

        foreach (var e in entries)
        {
            if (e == null || string.IsNullOrWhiteSpace(e.key) || e.flag == null) continue;
            map[e.key.Trim().ToLower()] = e.flag;
        }
    }

    public void Unlock(string key)
    {
        var flag = GetFlag(key);
        if (flag != null) flag.value = true;
    }

    public void Lock(string key)
    {
        var flag = GetFlag(key);
        if (flag != null) flag.value = false;
    }

    private BoolValue GetFlag(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return null;

        key = key.Trim().ToLower();
        if (map != null && map.TryGetValue(key, out var flag)) return flag;

        Debug.LogWarning($"DialogueUnlockRegistry: key '{key}' not found.");
        return null;
    }
}