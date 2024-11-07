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
        if (CheckManager.Instance != null) {
            CheckManager.Instance.RegisterItem(this);  // 注册到 CheckManager
        }
        
    }

    public bool Check(Vector3 position, Vector3 eulerAngle)
    {
        position.y = 0;
        float min_distance = 4;
        int index = -1, numRotated = (360 -(int)eulerAngle.y) % 360 / 45;
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
            CheckManager.Instance.CheckWinCondition();  
            numChecked++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Check() {
        if (numChecked < results.Length) {
            results[numChecked++] = true;
        }
        CheckManager.Instance.CheckWinCondition();  
    }

    public bool AreAllChecksTrue()
    {
        return numChecked == results.Length;
    }
}
