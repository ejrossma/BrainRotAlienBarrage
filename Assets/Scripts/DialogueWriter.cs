using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class DialogueWriter : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] string[] lines;
    public bool startDialogue;
    private int currentIndex;
    private bool finishedLine;

    [Header("Typing Control")]
    [SerializeField] float typeSpeed;
    [SerializeField] float waitTimeBetweenDialogue;
    private float waitTimeTimer;

    [Header("References")]
    [SerializeField] TMP_Text outputText;

    
    
    private void Update()
    {
        if (startDialogue)
        {
            StartCoroutine("TypeDialogue");
            startDialogue = false;
        }

        if (finishedLine)
        {
            waitTimeTimer += Time.deltaTime;
            if (waitTimeTimer > waitTimeBetweenDialogue)
            {
                outputText.text = "";
                currentIndex++;
                finishedLine = false;
                waitTimeTimer = 0;
                if (currentIndex == lines.Length)
                {
                    gameObject.SetActive(false);
                    return;
                }
                StartCoroutine("TypeDialogue");
            }
        }
    }

    IEnumerator TypeDialogue()
    {
        foreach (char c in lines[currentIndex]) 
        {
            outputText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        finishedLine = true;
    }
}
