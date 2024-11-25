using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingComponent : MonoBehaviour, IPointerEnterHandler, ICanvasRaycastFilter
{
    Selectable selectable;

    public Player player;


    public enum SettingType { MoveSpeed, Sensitivity, Volume}
    public SettingType settingType; 

    
    private Slider slider;

    private float minValue = 0.1f;
    private float maxValue = 10f;

    
    void OnSliderValueChanged(float value)
    {
        switch (settingType)
        {
            case SettingType.MoveSpeed:
                player.moveSpeed = value;
                break;
            case SettingType.Sensitivity:
                player.rotateSpeed = value;
                break;
            case SettingType.Volume:
                break;
        }
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        Texture2D texture = selectable.image.sprite.texture;
        RectTransform rectTransform = selectable.image.rectTransform;
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out localPoint))
            return false;

        Rect rect = rectTransform.rect;
        float x = (localPoint.x - rect.x) / rect.width;
        float y = (localPoint.y - rect.y) / rect.height;

        int pixelX = Mathf.RoundToInt(x * texture.width);
        int pixelY = Mathf.RoundToInt(y * texture.height);

        if (pixelX < 0 || pixelX >= texture.width || pixelY < 0 || pixelY >= texture.height)
            return false;

        return texture.GetPixel(pixelX, pixelY).a > 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectable.Select();
    }

    void Start()
    {
        selectable = GetComponent<Selectable>();

        
        slider = GetComponent<Slider>();

        if (! slider) return;

        switch (settingType)
        {
            case SettingType.MoveSpeed:
                slider.value = player.moveSpeed; 
                break;
            case SettingType.Sensitivity:
                slider.value = player.rotateSpeed; 
                break;
            case SettingType.Volume:
                break;
            default:
                break;
        }

        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
}
