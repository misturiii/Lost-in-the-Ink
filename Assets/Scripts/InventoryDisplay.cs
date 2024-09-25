using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Add this at the top

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

            // Add event triggers for dragging
            EventTrigger eventTrigger = icon.AddComponent<EventTrigger>();
            EventTrigger.Entry entryBeginDrag = new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag };
            entryBeginDrag.callback.AddListener((data) => { OnBeginDrag(icon); });
            eventTrigger.triggers.Add(entryBeginDrag);

            EventTrigger.Entry entryDrag = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
            entryDrag.callback.AddListener((data) => { OnDrag(icon); });
            eventTrigger.triggers.Add(entryDrag);

            // Calculate position
            int row = i / itemsPerRow;
            int column = i % itemsPerRow;
            float xPos = column * (icon.GetComponent<RectTransform>().rect.width + spacing);
            float yPos = -row * (icon.GetComponent<RectTransform>().rect.height + spacing);
            icon.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos); // Set position
        }
    }

    // New methods for drag functionality
    private Vector2 originalPosition; // Store original position

    private void OnBeginDrag(GameObject icon)
    {
        // Store the original position of the icon
        originalPosition = icon.GetComponent<RectTransform>().anchoredPosition;

        // Change the parent to the Canvas to ensure it is on top
        icon.transform.SetParent(inventoryContent.parent);
    }

    private void OnDrag(GameObject icon)
    {
        // Logic for dragging the icon
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(inventoryContent.GetComponent<RectTransform>(), Input.mousePosition, null, out mousePosition);
        icon.GetComponent<RectTransform>().anchoredPosition = mousePosition;
    }

    private void OnEndDrag(GameObject icon)
    {
        // Check if the icon is still in the inventoryContent
        if (icon.transform.parent == inventoryContent.parent)
        {
            // Reset the icon's position to the original position
            icon.GetComponent<RectTransform>().anchoredPosition = originalPosition;
        }
        else
        {
            // Optionally, you can handle dropping the icon in a new position here
        }

        // Reset the parent back to inventoryContent
        icon.transform.SetParent(inventoryContent);
    }
}
