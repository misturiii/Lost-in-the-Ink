using System.Collections.Generic;
using UnityEngine;

public class ImagePieceManager : MonoBehaviour
{
    [System.Serializable]
    public class ImagePiece
    {
        public string pieceName;
        public GameObject pieceObject; // Reference to the GameObject with SpriteRenderer
    }

    public List<ImagePiece> imagePieces = new List<ImagePiece>();

    private void Start()
    {
        // Hide all pieces at the start
        foreach (ImagePiece piece in imagePieces)
        {
            piece.pieceObject.SetActive(false);
        }
    }

    // Method to enable a specific piece
    public void ShowImagePiece(string pieceName)
    {
        foreach (ImagePiece piece in imagePieces)
        {
            if (piece.pieceName == pieceName)
            {
                piece.pieceObject.SetActive(true); // Enable the GameObject with SpriteRenderer
                Debug.Log(pieceName + " is now visible.");
                break;
            }
        }
    }
}
