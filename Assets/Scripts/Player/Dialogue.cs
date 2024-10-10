using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class Dialogue : MonoBehaviour
{

    public TextMeshProUGUI textComponent; // The TextMeshPro component for displaying dialogue
    public TextMeshProUGUI speakerCompoent;
    public DialogueObject dialogueObject;
    float textSpeed = 0.05f; // Speed of text typing
    String curLine;
    bool inProgress;

    void Start()
    {
        textComponent.text = string.Empty; // Clear text at start
        gameObject.SetActive(false); // Hide the dialogue box initially
        curLine = null;
        inProgress = false;
    }

    public void DsiplayDialogue()
    {
        if (dialogueObject) {
            gameObject.SetActive(true);
            if (inProgress) {
                // StopCoroutine(TypeLine());
                inProgress = false;
                textComponent.text = curLine;
                inProgress = false;
            } else {
                if (!dialogueObject.IsOver()) {
                    (speakerCompoent.text, curLine) = dialogueObject.CurrentLine(); 
                    StartCoroutine(TypeLine());
                } else {
                    gameObject.SetActive(false);
                }
            } 
        }
    }

    IEnumerator TypeLine()
    {
        inProgress = true;
        textComponent.text = string.Empty; // Ensure text is cleared before typing the current line
        // Type out each character one by one

        foreach (char c in curLine.ToCharArray())
        {
            if(inProgress){
                textComponent.text += c; // Append character
                yield return new WaitForSeconds(textSpeed); // Wait before typing the next character

            }
        }
        inProgress = false;
    }
}