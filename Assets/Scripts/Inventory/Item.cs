using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;    // Name of the item
    public int count = 1;
    public int total = 1;
    public GameObject prefab;
    public bool isTool = false;
    public bool stickerPlaced = false;

    [SerializeField] Vector3[] checks;
    bool[] results;

    public void Clear () {
        total = count = isTool ? -1 : 0;
        stickerPlaced = false;
        if (checks != null) {
            results = new bool[checks.Length];
        }
        Array.Fill(results, false);
    }

    void OnEnable () {
        Clear();
    }

    public bool Check (Vector3 position) {
        position.y = 0;
        float min_distance = Mathf.Infinity;
        int index = -1;
        for (int i = 0; i < checks.Length; i++) {
            float distance = (position - checks[i]).magnitude;
            if (distance < 4 && !results[i]) {
                min_distance = distance;
                index = i;
            }
        }
        if (index != -1) {
            results[index] = true;
            return true;
        } else {
            return false;
        }
    }
}
