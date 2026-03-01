using UnityEngine;

public abstract class ObjectiveDefinition : ScriptableObject
{
    [TextArea] public string uiText;

    public abstract bool IsSatisfied(PlayerInventory inv);
    public abstract string GetProgressText(PlayerInventory inv);
}
