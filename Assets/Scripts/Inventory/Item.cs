using UnityEditor.SearchService;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;    // Name of the item
    public int count = 1;
    public int total = 1;
    public GameObject prefab;
    public bool isTool = false;
    bool stickerPicked = false;

    public void Clear () {
        total = count = isTool ? -1 : 0;
        stickerPicked = false;
    }

    public void Picked () {
        stickerPicked = true;
    }

    public bool IsPicked => stickerPicked;

    void OnEnable () {
        Clear();
    }
}
