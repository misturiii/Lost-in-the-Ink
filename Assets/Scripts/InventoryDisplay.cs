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

        // Define grid parameters
        int itemsPerRow = 5; // Number of items per row
        float spacing = 10f; // Space between icons

        // Create icons for each item in the inventory
        for (int i = 0; i < inventory.items.Count; i++)
        {
            Item item = inventory.items[i];
            GameObject icon = Instantiate(itemIconPrefab, inventoryContent); // Create a new icon from prefab
            Image iconImage = icon.GetComponent<Image>(); // Get the Image component
            iconImage.sprite = item.icon; // Assign the item's icon sprite

            // Calculate position
            int row = i / itemsPerRow;
            int column = i % itemsPerRow;
            float xPos = column * (icon.GetComponent<RectTransform>().rect.width + spacing);
            float yPos = -row * (icon.GetComponent<RectTransform>().rect.height + spacing);
            icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos); // Set position
        }
    }
}
