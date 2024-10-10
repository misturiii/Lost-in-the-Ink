using UnityEngine;
using UnityEngine.InputSystem; // 引入新的输入系统命名空间

public class PlayerInteraction : MonoBehaviour
{
    public Dialogue dialogue;             // Reference to the Dialogue component
    private bool isPlayerInRange;         // Flag to check if player is in range
    private bool isDialogueTriggered;     

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Npc")) // Check if the collider is the NPC
        {
            isPlayerInRange = true; // Set flag to true
            Debug.Log("You are near the NPC. Press 'T' or L1 to interact."); // Debug message
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Npc"))
        {
            isPlayerInRange = false; // Reset flag when player exits range
            isDialogueTriggered = false; 
            Debug.Log("You have left the interaction range."); // Debug message
        }
    }

    private void Update()
    {
        
        if (dialogue.isInitialComplete && isPlayerInRange && !isDialogueTriggered &&
            (Input.GetKeyDown(KeyCode.T) || (Gamepad.current != null && Gamepad.current.leftShoulder.wasPressedThisFrame)))
        {
            
            TriggerDialogueInteraction();
        }

        
        if (dialogue.started)
        {
           
            if (Input.GetMouseButtonDown(0) || (Gamepad.current != null && Gamepad.current.leftTrigger.wasPressedThisFrame))
            {
                ContinueDialogue();
            }
        }
    }

    
    private void TriggerDialogueInteraction()
    {
        if (!dialogue.started)
        {
            
            dialogue.gameObject.SetActive(true);
            dialogue.SetInteracted(); 
            dialogue.started = true;  
            isDialogueTriggered = true; 

            Debug.Log("Dialogue interaction started with NPC.");
        }
    }

    
    private void ContinueDialogue()
    {
        if (dialogue.started)
        {
            dialogue.NextLine();  
            Debug.Log("Continuing dialogue...");
        }
    }
}
