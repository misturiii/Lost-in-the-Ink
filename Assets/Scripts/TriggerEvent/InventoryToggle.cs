using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;  // Reference to the inventory panel
    public GameObject inventoryPanel2;  // Reference to the inventory panel

    private bool isInventoryOpen = false;  // Track whether the inventory is open
    InputActions inputActions;

    void Awake () {
        inputActions = new InputActions();
        inputActions.UI.Enable();
        inputActions.UI.Trigger.performed += ToggleInventory;
    }

    // Method to toggle the inventory visibility
    void ToggleInventory(InputAction.CallbackContext context)
    {
        isInventoryOpen = !isInventoryOpen;  // Toggle the inventory state
        inventoryPanel.SetActive(isInventoryOpen);  // Show or hide the panel based on the state
        inventoryPanel2.SetActive(isInventoryOpen);  // Show or hide the panel based on the state
    }
}
