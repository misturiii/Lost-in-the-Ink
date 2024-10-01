using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Dialogue dialogue; // Reference to the Dialogue component
    private bool isPlayerInRange; // Flag to check if player is in range

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Npc")) // Check if the collider is the girl
        {
            isPlayerInRange = true; // Set flag to true
            Debug.Log("You are near the girl. Press 'T' to interact."); // Debug message
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Npc"))
        {
            isPlayerInRange = false; // Reset flag when player exits range
            Debug.Log("You have left the interaction range."); // Debug message
        }
    }

    private void Update()
    {
        if (dialogue.isInitialComplete && isPlayerInRange && Input.GetKeyDown(KeyCode.T))
        {
            dialogue.gameObject.SetActive(true);
            dialogue.SetInteracted();
        }

        // Proceed to the next line on left click
        if (dialogue.started && Input.GetMouseButtonDown(0))
        {
            dialogue.NextLine(); 
        }
    }
}
