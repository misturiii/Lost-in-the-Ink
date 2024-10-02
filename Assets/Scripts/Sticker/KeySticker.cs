using UnityEngine;
using UnityEngine.EventSystems;

public class BlueSticker : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform purpleBox;   // The purple box (target area)
    [SerializeField] float proximityThreshold = 10f;  // Distance threshold for proximity detection
    [SerializeField] GameObject airWall;  // Reference to the airwall in the 3D scene
    
    private RectTransform blueStickerRect;  // RectTransform of the blue sticker

    void Start()
    {
        blueStickerRect = GetComponent<RectTransform>();  // Get the RectTransform of this sticker (the blue sticker)
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update sticker's position during drag
        transform.position += (Vector3)eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if the blue sticker is near the purple box
        if (IsStickerNearPurpleBox())
        {
            // Disable the airwall when the blue sticker is near the purple box
            DisableAirWall();
        }
    }

    private bool IsStickerNearPurpleBox()
    {
        // Calculate the distance between the blue sticker and the purple box
        float distance = Vector2.Distance(blueStickerRect.anchoredPosition, purpleBox.anchoredPosition);

        // Check if the distance is less than the defined proximity threshold
        return distance < proximityThreshold;
    }

    private void DisableAirWall()
    {
        if (airWall != null)
        {
            airWall.SetActive(false);  // Disable the airwall
            Debug.Log("Airwall disabled.");
        }
        else
        {
            Debug.LogError("Airwall reference is missing.");
        }
    }
}
