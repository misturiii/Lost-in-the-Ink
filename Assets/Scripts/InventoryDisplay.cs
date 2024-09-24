using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public Inventory inventory;             // Reference to the inventory
    public GameObject itemIconPrefab;       // Prefab for item icons
    public Transform inventoryContent;      // Parent object for inventory items

    // Method to update the inventory UI when items are added
    public void UpdateInventoryDisplay()
    {
        Debug.Log("Updating Inventory Display...");

        // Check if inventory is assigned
        if (inventory == null)
        {
            Debug.LogError("Inventory is not assigned!");
            return;
        }

        // Clear previous icons
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // Log the number of items in the inventory
        Debug.Log("Inventory contains " + inventory.items.Count + " items.");

        // Create icons for each item in the inventory
        foreach (Item item in inventory.items)
        {
            if (item.icon == null)
            {
                Debug.LogError("Item " + item.itemName + " does not have an icon assigned.");
                continue;
            }

            Debug.Log("Displaying icon for item: " + item.itemName);
            GameObject icon = Instantiate(itemIconPrefab, inventoryContent); // Create a new icon from prefab

            Image iconImage = icon.GetComponent<Image>(); // Get the Image component
            if (iconImage != null)
            {
                iconImage.sprite = item.icon; // Assign the item's icon sprite
                Debug.Log("Icon successfully assigned for: " + item.itemName);
            }
            else
            {
                Debug.LogError("Item icon prefab is missing an Image component!");
            }
        }
    }
}
