using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel;  // Reference to the inventory panel
    public GameObject inventoryPanel2;  // Reference to the inventory panel
    public KeyCode toggleKey = KeyCode.I;  // Default key to open/close the inventory

    private bool isInventoryOpen = false;  // Track whether the inventory is open

    void Update()
    {
        // Check if the player presses the inventory key
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }
    }

    // Method to toggle the inventory visibility
    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;  // Toggle the inventory state
        inventoryPanel.SetActive(isInventoryOpen);  // Show or hide the panel based on the state
        inventoryPanel2.SetActive(isInventoryOpen);  // Show or hide the panel based on the state
    }
}
