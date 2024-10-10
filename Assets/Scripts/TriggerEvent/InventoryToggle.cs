using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;  // Reference to the inventory panel
    public GameObject inventoryPanel2;  // Reference to the second inventory panel (if any)
    // public GameObject inventoryChat;

    private bool isInventoryOpen = false;  // Track whether the inventory is open
    InputActions inputActions;

    void Start()
    {
        // Get InputActionManager and UI's InputActions
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Trigger.performed += ToggleInventory;
        inputActions.Player.Trigger.performed += ToggleInventory;
    }

    // Method to toggle the inventory visibility
    void ToggleInventory(InputAction.CallbackContext context)
    {
        isInventoryOpen = !isInventoryOpen;  // Toggle the inventory state
        inventoryPanel.SetActive(isInventoryOpen);  // Show or hide the panel based on the state
        inventoryPanel2.SetActive(isInventoryOpen);  // Show or hide the panel based on the state
    }
}
