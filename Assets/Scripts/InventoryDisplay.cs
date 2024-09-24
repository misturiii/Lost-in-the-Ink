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
        // Clear previous icons
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // Create icons for each item in the inventory
        foreach (Item item in inventory.items)
        {
            GameObject icon = Instantiate(itemIconPrefab, inventoryContent); // Create a new icon from prefab
            Image iconImage = icon.GetComponent<Image>(); // Get the Image component
            iconImage.sprite = item.icon; // Assign the item's icon sprite
        }
    }
}
