using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;    // Name of the item
    public Sprite icon;        // Icon of the item to be displayed in the inventory
    public string itemType; // Add this line
    public GameObject prefab;
}
