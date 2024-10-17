using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI; // For Image
using TMPro; // For TextMeshPro

public class PickupObject : MonoBehaviour
{
    Inventory inventory;                // Reference to the Inventory ScriptableObject
    public InventoryDisplay inventoryDisplay;  // Reference to the InventoryDisplay
    private GameObject currentItem;      // Track the currently detected item
    InputActions inputActions;

    public Image pickupBackground;       // Reference to the background image
    public TextMeshProUGUI pickupText;   // Reference to the text component
    public Image controllerGuide;
    void Start()
    {
        inventory = Resources.Load<Inventory>("PlayerInventory");
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.GrabSticker.Enable();
        inputActions.Player.GrabSticker.performed += Grab;

        // Initially hide the pickup text and background
        if (pickupBackground != null && pickupText != null && controllerGuide !=null)
        {
            pickupBackground.enabled = false;
            pickupText.enabled = false;
            controllerGuide.enabled = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Check if the object is tagged as "PickableItem"
        if (other.CompareTag("PickableItem"))
        {
            currentItem = other.gameObject; // Store the currently detected item
            Debug.Log("Detected item: " + currentItem.name);

            // Show the pickup text and background
            ShowPickupGuide();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Clear the current item when the player exits the trigger area
        if (other.CompareTag("PickableItem"))
        {
            Debug.Log("Exited item: " + currentItem.name);
            currentItem = null;

            // Hide the pickup text and background
            HidePickupGuide();
        }
    }

    void Grab(InputAction.CallbackContext context)
    {
        // Check for the E key press and if there's a current item
        if (context.performed && currentItem != null)
        {
            ItemObject itemObject = currentItem.GetComponent<ItemObject>();
            if (itemObject != null)
            {
                inventory.Add(itemObject.item); // Add the item to the inventory
                Debug.Log("Picked up item: " + itemObject.item.itemName);

                // Destroy the item from the scene
                Destroy(currentItem);
                currentItem = null; // Reset current item

                // Hide the pickup text and background after pickup
                HidePickupGuide();
            }
            else
            {
                Debug.LogError("ItemObject component is missing on: " + currentItem.name);
            }
        }
    }

    // Method to manually check if player is in the pickup area
    public void CheckForPickupItem()
    {
        if (currentItem != null && currentItem.CompareTag("PickableItem"))
        {
            // Manually display pickup guide if player is in the area after dialogue ends
            Debug.Log("Player is in the pickup area after dialogue ended, item is now pickable.");
            ShowPickupGuide();
        }
    }

    // Show the pickup guide (text and background)
    private void ShowPickupGuide()
    {
        if (pickupBackground != null && pickupText != null && controllerGuide != null)
        {
            pickupBackground.enabled = true;
            pickupText.enabled = true;
            controllerGuide.enabled = true;
            pickupText.text = "Press E to pickup           or";  // Set the text
        }
    }

    // Hide the pickup guide (text and background)
    private void HidePickupGuide()
    {
        if (pickupBackground != null && pickupText != null && controllerGuide != null)
        {
            pickupBackground.enabled = false;
            pickupText.enabled = false;
            controllerGuide.enabled = false;
        }
    }
}
