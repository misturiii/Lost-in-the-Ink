using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;  // Reference to the inventory panel
    // public GameObject inventoryChat;
    public AudioSource audioSource;    // Reference to the AudioSource component
    public AudioClip openSound;        // Sound to play when the inventory is opened
    public AudioClip closeSound;       // Sound to play when the inventory is closed

    private bool isInventoryOpen = false;  // Track whether the inventory is open
    InputActionManager inputActionManager;
    [SerializeField] GameObject current;


    void Start()
    {
        // Get InputActionManager and UI's InputActions
        inputActionManager = FindObjectOfType<InputActionManager>();
        inputActionManager.inputActions.UI.Trigger.performed += ToggleInventory;
        inputActionManager.inputActions.Player.Trigger.performed += ToggleInventory;
    }

    // Method to toggle the inventory visibility
    void ToggleInventory(InputAction.CallbackContext context)
    {
        ToggleInventory();
    }

    public void ToggleInventory() {
        isInventoryOpen = !isInventoryOpen;  // Toggle the inventory state
        inventoryPanel.SetActive(isInventoryOpen);  // Show or hide the panel based on the state
        if (isInventoryOpen) {
            if (audioSource != null && openSound != null)
            {
                audioSource.PlayOneShot(openSound);  // Play the open sound
            }
        } else {
            if (audioSource != null && closeSound != null)
            {
                audioSource.PlayOneShot(closeSound);  // Play the close sound
            }
            CheckManager.Instance.OnSketchbookClosed();
        }
        inputActionManager.SetPlayerActive(!isInventoryOpen);
    }
}
