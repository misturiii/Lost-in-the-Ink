using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;  // Reference to the inventory panel
    public GameObject inventoryPanel2;  // Reference to the second inventory panel (if any)

    private bool isInventoryOpen = false;  // Track whether the inventory is open
    InputActions inputActions;
    private InputAction moveAction;  // Reference to the Player's Move action
    private InputAction lookAction;  // Reference to the Player's Look action

    void Start()
    {
        // Get InputActionManager and UI's InputActions
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Enable();
        inputActions.UI.Trigger.performed += ToggleInventory;

        // Get Player action Move and Look Action
        moveAction = inputActions.Player.Move;
        lookAction = inputActions.Player.Look;
    }

    // Method to toggle the inventory visibility
    void ToggleInventory(InputAction.CallbackContext context)
    {
        isInventoryOpen = !isInventoryOpen;  // Toggle the inventory state
        inventoryPanel.SetActive(isInventoryOpen);  // Show or hide the panel based on the state
        inventoryPanel2.SetActive(isInventoryOpen);  // Show or hide the panel based on the state

        if (isInventoryOpen)
        {
            // fobidden Player action Move and Look Action
            moveAction.Disable();
            lookAction.Disable();
        }
        else
        {
            // start Player action Move and Look Action
            moveAction.Enable();
            lookAction.Enable();
        }
    }
}
