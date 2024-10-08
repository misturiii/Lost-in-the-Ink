using UnityEngine;
using UnityEngine.InputSystem;

public class PickupObject : MonoBehaviour
{
    Inventory inventory;              // Reference to the Inventory ScriptableObject
    public InventoryDisplay inventoryDisplay; // Reference to the InventoryDisplay
    private GameObject currentItem;          // Track the currently detected item
    InputActions inputActions;

    void Start () {
        inventory = Resources.Load<Inventory>("PlayerInventory");
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.GrabSticker.Enable();
        inputActions.Player.GrabSticker.performed += Grab;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is tagged as "PickableItem"
        if (other.CompareTag("PickableItem"))
        {
            currentItem = other.gameObject; // Store the currently detected item
            Debug.Log("Detected item: " + currentItem.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Clear the current item when the player exits the trigger area
        if (other.CompareTag("PickableItem"))
        {
            Debug.Log("Exited item: " + currentItem.name);
            currentItem = null;
        }
    }

    void Grab (InputAction.CallbackContext context)
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
            }
            else
            {
                Debug.LogError("ItemObject component is missing on: " + currentItem.name);
            }
        }
    }
}