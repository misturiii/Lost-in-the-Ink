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
