using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StickerTaskbar : MonoBehaviour
{
    [SerializeField] private TMP_Text taskbarText; // Reference to the UI text element
    [SerializeField] private int totalStickers = 11; // Total number of stickers to be collected
    private ImagePieceManager imagePieceManager;
    public AudioSource audioSource;
    public AudioClip ImageAppear;
    private int collectedStickers = 0;

    private void Start()
    {
        imagePieceManager = FindObjectOfType<ImagePieceManager>(); 
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
        if(collectedStickers>6){
            imagePieceManager.ShowImagePiece("BallonCart");
            audioSource.PlayOneShot(ImageAppear);
        }else if(collectedStickers>5){
            imagePieceManager.ShowImagePiece("IceCream");
            audioSource.PlayOneShot(ImageAppear);

        }else if(collectedStickers>4){
            imagePieceManager.ShowImagePiece("FerrisWheel");
            audioSource.PlayOneShot(ImageAppear);
        }else if(collectedStickers>3){
            imagePieceManager.ShowImagePiece("Circus");
            audioSource.PlayOneShot(ImageAppear);
        }else if(collectedStickers>2){
            imagePieceManager.ShowImagePiece("Fountain");
            audioSource.PlayOneShot(ImageAppear);
        }else if (collectedStickers>1){
            imagePieceManager.ShowImagePiece("Benches");
            audioSource.PlayOneShot(ImageAppear);
        }

    }
}