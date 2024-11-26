// using UnityEngine;
// using TMPro;
// using System;

// public class StickerTaskbar : MonoBehaviour
// {
//     [SerializeField] private TMP_Text taskbarText; // Reference to the UI text element
//     [SerializeField] private int totalStickers = 11; // Total number of stickers to be collected
//     private ImagePieceManager imagePieceManager;
//     public AudioSource audioSource;
//     public AudioClip ImageAppear;
//     private int collectedStickers = 0;
//     LightManager lightManager;
//     String[] mapNames = {"Benches", "Fountain", "Circus", "FerrisWheel", "IceCream", "BallonCart"};

//     private void Start()
//     {
//         imagePieceManager = FindObjectOfType<ImagePieceManager>(); 
//         lightManager=FindObjectOfType<LightManager>();
//         UpdateTaskbar();
//     }

//     // Call this function whenever a sticker is collected
//     public void AddSticker()
//     {
//         collectedStickers++;
//         UpdateTaskbar();
//     }

//     private void UpdateTaskbar()
//     {
//         // Update the text to show the current count
//         taskbarText.text = $"Stickers: {collectedStickers}/{totalStickers}";
//         if (collectedStickers > 0 && collectedStickers <= 6) {
//             MapAppear(collectedStickers - 1);
//         }
//     }

//     void MapAppear (int index) {
//         imagePieceManager.ShowImagePiece(mapNames[index]);
//         audioSource.PlayOneShot(ImageAppear);
//         lightManager.TurnOnLight();
//     }
// }

using UnityEngine;
using TMPro;
using System;

public class StickerTaskbar : MonoBehaviour
{
    [SerializeField] private TMP_Text taskbarText; // Reference to the UI text element
    [SerializeField] private int totalStickers = 13; // Total number of stickers to be collected
    private ImagePieceManager imagePieceManager;
    public AudioSource audioSource;
    public AudioClip ImageAppear;
    private int collectedStickers = 0;
    LightManager lightManager;

    // Array to store the map names that correspond to specific sticker counts
    private String[] mapNames = { "Circus", "Benches", "Fountain", "FerrisWheel", "IceCream", "BallonCart" };

    // Array to define at which sticker counts the map pieces should appear (1, 3, 5, 7, 9, 11)
    private int[] stickerAppearThresholds = { 1, 3, 5, 7, 9, 11 };

    private void Start()
    {
        imagePieceManager = FindObjectOfType<ImagePieceManager>();
        lightManager = FindObjectOfType<LightManager>();
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

        // Check if the collected stickers match one of the thresholds for showing a new map piece
        for (int i = 0; i < stickerAppearThresholds.Length; i++)
        {
            if (collectedStickers == stickerAppearThresholds[i])
            {
                int index = i; // Use the index to select the correct map piece
                MapAppear(index);
                break; // No need to check further if one match is found
            }
        }
    }

    void MapAppear(int index)
    {
        if (index < mapNames.Length)
        {
            imagePieceManager.ShowImagePiece(mapNames[index]);
            audioSource.PlayOneShot(ImageAppear);
            lightManager.TurnOnLight();
        }
    }
}
