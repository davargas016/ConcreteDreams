using UnityEngine;
using TMPro;

public class DialogueObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myText;

    public void Setup(string newDialogue)
    {
        myText.text = newDialogue;
    }

    void Start() { }
    void Update() { }
}