using UnityEngine;
using TMPro;

public class ResponseObject : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI myText;
    private int choiceValue;

    public void Setup(string newDialogue, int myChoice)
    {
        myText.text = newDialogue;
        choiceValue = myChoice;
    }

    void Start() { }
    void Update() { }
}