using UnityEngine;
using TMPro;
using System;

public class StickerTaskbar : MonoBehaviour
{
    [SerializeField] private TMP_Text taskbarText; // Reference to the UI text element
    [SerializeField] private int totalStickers = 11; // Total number of stickers to be collected
    private ImagePieceManager imagePieceManager;
    public AudioSource audioSource;
    public AudioClip ImageAppear;
    private int collectedStickers = 0;
    LightManager lightManager;
    String[] mapNames = {"Benches", "Fountain", "Circus", "FerrisWheel", "IceCream", "BallonCart"};

    private void Start()
    {
        imagePieceManager = FindObjectOfType<ImagePieceManager>(); 
        lightManager=FindObjectOfType<LightManager>();
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
        if (collectedStickers > 0 && collectedStickers <= 6) {
            MapAppear(collectedStickers - 1);
        }
    }

    void MapAppear (int index) {
        imagePieceManager.ShowImagePiece(mapNames[index]);
        audioSource.PlayOneShot(ImageAppear);
        lightManager.TurnOnLight();
    }
}