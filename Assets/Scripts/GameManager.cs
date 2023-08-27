using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject dialogueInterface;
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject loseScreen;
    [SerializeField] bool skipDialogue;

    // Start is called before the first frame update
    void Start()
    {
        if (!skipDialogue)
        {
            StartDialogue();
            player.GetComponent<Player>().hasControl = false;
        }
        else
        {
            FinishIntro();
        }
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

    public void Lose()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        loseScreen.SetActive(true);
        playerHUD.SetActive(false);
    }
}
