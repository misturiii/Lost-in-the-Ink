using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;    // Name of the item
    public int count;
    public int total;
    public GameObject prefab;
    public bool isTool = false;
    public Tool toolType;
    public bool stickerPlaced = false;

    [SerializeField] Vector3[] checks;
    [SerializeField] int[] rotations;
    bool[] results;
    int numChecked;

    public string compatibleItems;

    public void Clear()
    {
        total = count = isTool ? -1 : 0;
        numChecked = 0;
        stickerPlaced = false;
        if (checks != null) {
            results = new bool[Mathf.Min(checks.Length, rotations.Length)];
            Array.Fill(results, false);
        }
    }

    void OnEnable()
    {
        Clear();
        Debug.Log("Item awake");
        if (CheckManager.Instance != null) {
            CheckManager.Instance.RegisterItem(this);  // 注册到 CheckManager
        }
    }

    public void RemovePieceCheck () {
        CheckManager.Instance.RemovePieceCheck(this);
    }

    public int Check(Vector3 position, Vector3 eulerAngle)
    {
        position.y = 0;
        float min_distance = 10;
        int index = -1, numRotated = (360 -(int)eulerAngle.y) % 360 / 90;
        Debug.Log("There were " + numRotated + " rotation when checked");
        for (int i = 0; i < results.Length; i++)
        {
            float distance = (position - checks[i]).magnitude;
            if (distance < min_distance && !results[i] && numRotated == rotations[i])
            {
                min_distance = distance;
                index = i;
            }
        }
        if (index != -1)
        {
            results[index] = true;
            Debug.Log($"{itemName} check {index} set to true.");
            numChecked++;
            CheckManager.Instance.CheckWinCondition();  
        }
        return index;
    }

    public int Check() {
        if (numChecked < results.Length) {
            results[numChecked++] = true;
            return numChecked -1;
        }
        CheckManager.Instance.CheckWinCondition();  
        return -1;
    }

    public bool AreAllChecksTrue()
    {
        return numChecked == results.Length;
    }

    public void Uncheck (int index) {
        if (index >= 0) {
            results[index] = false;
            numChecked--;
        }
    }
}
