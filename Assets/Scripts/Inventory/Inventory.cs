using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> tools = new List<Item>();
    public List<Item> items = new List<Item>();
    public event Action OnEmptyInventory;
    public event Action OnContainSticker;
    public string newItem;

    // Method to add an item to the inventory
    public void Add(Item item)
    {
        if (item.isTool) {
            tools.Add(item);
        } else if (!items.Contains(item)) {
            items.Add(item);
        } 
        item.count++;
        item.total++;
        Debug.Log(item.itemName + " added to inventory");
        Debug.Log("Inventory contains the following stickers:");
        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log("Sticker: " + items[i].name); // Assuming 'name' is a property in your Item class
        }
        newItem = item.itemName;
        OnContainSticker?.Invoke();
    }

    public void Remove (int index) {
        Debug.Log($"remove item at index {index}");
        items.Remove(items[index]);
        if (items.Count == 0) {
            OnEmptyInventory?.Invoke();
        }
    }

    // New method to clear the inventory
    public void Clear()
    {
        items.Clear();
        tools.Clear();
        Debug.Log("Inventory cleared");
    }

    // This method will be called when the game starts or when the ScriptableObject is loaded
    private void OnEnable()
    {
        Clear();
    }
}
