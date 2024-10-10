using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // The TextMeshPro component for displaying dialogue
    public string[] initialLines; // Lines to show before interaction
    public string[] followUpLines; // Lines to show after interaction
    public float textSpeed = 0.05f; // Speed of text typing
    private int index; // Current index of the line being displayed
    public bool canProceed; // Determines if the player can interact
    public bool isDialogueComplete; // Indicates if the current dialogue is complete
    public bool hasInteracted; // Tracks if the player has interacted
    public bool started; // Tracks if the dialogue has started
    public bool isInitialComplete; // Tracks if the initial dialogue is complete

    void Start()
    {
        textComponent.text = string.Empty; // Clear text at start
        gameObject.SetActive(false); // Hide the dialogue box initially
        StartDialogue(true);
    }

    public void StartDialogue(bool init)
    {
        index = 0; // Reset index
        canProceed = false; // Disable interaction initially
        isDialogueComplete = false; // Reset dialogue completion status
        started = true; // Set dialogue started flag
        if (init) {
            isInitialComplete = false;
            hasInteracted = false;
        }
        else{
            isInitialComplete = true;
            hasInteracted = true;
        }
        gameObject.SetActive(true); // Show the dialogue box
        textComponent.text = string.Empty; // Ensure text is cleared before typing new dialogue
        StartCoroutine(TypeLine()); // Start typing the first line
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty; // Ensure text is cleared before typing the current line
        // Type out each character one by one
        foreach (char c in GetCurrentDialogue()[index].ToCharArray())
        {
            textComponent.text += c; // Append character
            yield return new WaitForSeconds(textSpeed); // Wait before typing the next character
        }
        isDialogueComplete = true; // Mark dialogue as complete
        canProceed = true; // Allow interaction
    }

    public void NextLine()
    {
        // Move to the next line in the dialogue
        if (index < GetCurrentDialogue().Length - 1)
        {
            index++; // Increment the index
            textComponent.text = string.Empty; // Clear current text
            StartCoroutine(TypeLine()); // Start typing the next line
        }
        else
        {
            // End the dialogue when all lines are completed
            if (!isInitialComplete)
            {
                isInitialComplete = true; // Set initial dialogue completion status
            }
            started = false; // Reset started flag
            index = 0; // Reset index to 0 for follow-up dialogue
            gameObject.SetActive(false); // Hide dialogue box
        }
    }

    private string[] GetCurrentDialogue()
    {
        // Return the current dialogue based on interaction status
        return hasInteracted ? followUpLines : initialLines; 
    }

    public void SetInteracted()
    {   
        StartDialogue(false); // Start the follow-up dialogue
        hasInteracted = true; // Mark as interacted only if the initial dialogue is complete
        index = 0; // Reset the index to 0 for the follow-up dialogue
        textComponent.text = string.Empty; // Clear any previous text
    }
}
