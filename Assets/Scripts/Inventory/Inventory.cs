using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> items = new List<Item>();

    // Method to add an item to the inventory
    public void Add(Item item)
    {
        items.Add(item);
        Debug.Log(item.itemName + " added to inventory");
        Debug.Log("Inventory contains the following stickers:");
        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log("Sticker: " + items[i].name); // Assuming 'name' is a property in your Item class
        }
    }

    public void Remove (int index) {
        Debug.Log(items[0]);
        Debug.Log(index);
        items.Remove(items[index]);
    }

    // New method to clear the inventory
    public void Clear()
    {
        items.Clear();
        Debug.Log("Inventory cleared");
    }

    // This method will be called when the game starts or when the ScriptableObject is loaded
    private void OnEnable()
    {
        Clear();
    }
}
