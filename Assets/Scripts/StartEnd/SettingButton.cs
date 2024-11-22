using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour, IPointerEnterHandler, ICanvasRaycastFilter
{
    Button button;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        Texture2D texture = button.image.sprite.texture;

        // Convert screen point to local point within the image rect
        RectTransform rectTransform = button.image.rectTransform;
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out localPoint))
            return false;

        // Convert the local point to texture UV coordinates
        Rect rect = rectTransform.rect;
        float x = (localPoint.x - rect.x) / rect.width;
        float y = (localPoint.y - rect.y) / rect.height;

        // Convert UV coordinates to pixel coordinates
        int pixelX = Mathf.RoundToInt(x * texture.width);
        int pixelY = Mathf.RoundToInt(y * texture.height);

        if (pixelX < 0 || pixelX >= texture.width || pixelY < 0 || pixelY >= texture.height)
            return false;

        // Get the pixel color and check the alpha value
        return texture.GetPixel(pixelX, pixelY).a > 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.Select();
    }

    void Start()
    {
        button = GetComponent<Button>();
    }
}
