using System.Text;
using TMPro;
using UnityEngine;

public class QuestTrackerUI : MonoBehaviour
{
    [SerializeField] TMP_Text bodyText;

    bool subscribed = false;

    void Update()
    {
        // Late subscribe (fixes load-order / enable-order issues)
        if (!subscribed && QuestManager.I != null)
        {
            QuestManager.I.OnObjectivesChanged += Refresh;
            subscribed = true;
            Refresh();
        }
    }

    void OnDisable()
    {
        if (QuestManager.I != null && subscribed)
            QuestManager.I.OnObjectivesChanged -= Refresh;

        subscribed = false;
    }

    void Refresh()
    {
        if (bodyText == null) return;

        if (QuestManager.I == null)
        {
            bodyText.text = "";
            return;
        }

        var sb = new StringBuilder();
        foreach (var q in QuestManager.I.Active)
        {
            if (q?.def?.objective == null) continue;

            string label = q.def.objective.uiText;
            string progress = q.def.objective.GetProgressText(QuestManager.I.playerInventory);

            sb.AppendLine($"{label}: {progress}");
        }

        bodyText.text = sb.ToString().TrimEnd();
    }
}
