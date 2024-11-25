using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> items = new List<Item>();
    private List<Item> itemInsketchbook = new List<Item>();
    public event Action OnEmptyInventory;
    public event Action OnContainSticker;
    public string newItem;

    // Method to add an item to the inventory
    public void Add(Item item)
    {
        if (!items.Contains(item)) {
            items.Add(item);
        } 
        item.count++;
        item.total++;
        Debug.Log(item.itemName + " added to inventory");
        // call the method to remove
        GameObject npc = GameObject.Find("Jester_Animations"); // Add an 'associatedNPCName' property to the item
        if (npc != null)
        {
            NPC npcManager = npc.GetComponent<NPC>();
            if (npcManager != null)
            {
                npcManager.RemoveHint(item.itemName);
            }
        }
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
        itemInsketchbook.Add(items[index]);
        items.Remove(items[index]);
        if (items.Count == 0) {
            OnEmptyInventory?.Invoke();
        }
    }

    // New method to clear the inventory
    public void Clear()
    {
        foreach (var item in items) {
            item.Clear();
        } 
        foreach (var item in itemInsketchbook) {
            item.Clear();
        }
        items.Clear();
        itemInsketchbook.Clear();
        Debug.Log("Inventory cleared");
    }

    // This method will be called when the game starts or when the ScriptableObject is loaded
    void OnEnable()
    {
        Clear();
        Debug.Log("Inventory OnEnable");
    }
}
