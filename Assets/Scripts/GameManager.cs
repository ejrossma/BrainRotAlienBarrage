using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Opening Sequence")]
    [SerializeField] GameObject dialogueInterface;

    // Start is called before the first frame update
    void Start()
    {
        StartDialogue();
    }

    private void StartDialogue()
    {
        dialogueInterface.SetActive(true);
        dialogueInterface.GetComponent<DialogueWriter>().startDialogue = true;
    }
}
