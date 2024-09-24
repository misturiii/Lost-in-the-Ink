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
}
