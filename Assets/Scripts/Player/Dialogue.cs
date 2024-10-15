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
    string styleStart = "<b><color=#af001c>";
    string styleEnd = "</color></b>";
    
    private DialogueManager dialogueManager;  // 引用 DialogueManager

    void Start()
    {
        textComponent.text = string.Empty; // Clear text at start
        gameObject.SetActive(false); // Hide the dialogue box initially
        curLine = null;
        inProgress = false;
        dialogueManager = FindObjectOfType<DialogueManager>();  // 查找 DialogueManager 组件
    }

    public void DsiplayDialogue()
    {
        if (dialogueObject) {
            gameObject.SetActive(true);
            if (inProgress) {
                // StopCoroutine(TypeLine());
                inProgress = false;
            } else {
                if (!dialogueObject.IsOver()) {
                    (speakerCompoent.text, curLine) = dialogueObject.CurrentLine(); 
                    StartCoroutine(TypeLine());
                } else {
                    gameObject.SetActive(false);
                    OnDialogueFinish();  // 当对话结束时，调用这个方法
                }
            } 
        }
    }

    IEnumerator TypeLine()
    {
        inProgress = true;
        textComponent.text = string.Empty; // Ensure text is cleared before typing the current line
        // Type out each character one by one

        foreach (char c in curLine.ToCharArray()) {
            if (c == '<') {
                textComponent.text += styleStart;
            } else if (c == '>') {
                textComponent.text += styleEnd;
            } else {
                textComponent.text += c; // Append character
            }
            if (inProgress) {
                yield return new WaitForSeconds(textSpeed); // Wait before typing the next character
            }
        }
        inProgress = false;
    }

    // 当对话结束时调用的方法
    private void OnDialogueFinish()
    {
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueEnd();  // 通知 DialogueManager 对话结束
        }
        else
        {
            Debug.LogError("DialogueManager not found when trying to end dialogue.");
        }
    }
}
