using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StickerTaskbar : MonoBehaviour
{
    [SerializeField] private TMP_Text taskbarText; // Reference to the UI text element
    [SerializeField] private int totalStickers = 10; // Total number of stickers to be collected

    private int collectedStickers = 0;

    private void Start()
    {
        UpdateTaskbar();
    }

    // Call this function whenever a sticker is collected
    public void AddSticker()
    {
        collectedStickers++;
        UpdateTaskbar();
    }

    private void UpdateTaskbar()
    {
        // Update the text to show the current count
        taskbarText.text = $"Stickers: {collectedStickers}/{totalStickers}";
    }
}
