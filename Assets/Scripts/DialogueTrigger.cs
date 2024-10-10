using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; 

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger area
        if (other.CompareTag(" ")) 
        {
            dialogue.StartDialogue(true); // Start the dialogue
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Optional: Hide dialogue if player leaves the trigger area
        if (other.CompareTag("Player"))
        {
            dialogue.gameObject.SetActive(false); // Hide dialogue box
        }
    }
}