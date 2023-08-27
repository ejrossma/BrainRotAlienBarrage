using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Opening Sequence")]
    [SerializeField] GameObject dialogueInterface;
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        StartDialogue();
        player.GetComponent<Player>().hasControl = false;
    }

    private void StartDialogue()
    {
        dialogueInterface.SetActive(true);
        dialogueInterface.GetComponent<DialogueWriter>().startDialogue = true;
    }

    //intro has finished give player the reins
    public void FinishIntro()
    {
        dialogueInterface.SetActive(false);
        player.GetComponent<Player>().hasControl = true;
    }
}
