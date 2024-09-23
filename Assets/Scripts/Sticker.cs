using UnityEngine;
using UnityEngine.EventSystems;

public class Sticker : MonoBehaviour, IDragHandler, IDropHandler
{
    [SerializeField, Min(0)] float initialHeight = 5;
    [SerializeField, Min(0)] float sketchbookWidth = 40, sketchbookHeight = 30;  
    [SerializeField] GameObject prefab;
    RectTransform rectTransform;
    Transform copy;

    void Start () {
        rectTransform = GetComponent<RectTransform>();
        copy = null;
    }

    public void UseSticker() {
        float x = -rectTransform.localPosition.x / Screen.width * sketchbookWidth;
        float z = -rectTransform.localPosition.y / Screen.height * sketchbookHeight;
        Debug.Log(rectTransform.localPosition.x);
        if (!copy) {
            copy = Instantiate(prefab).transform;
        }
        copy.localPosition = new Vector3(x, initialHeight, z);
    }

    public void OnDrop(PointerEventData eventData)
    {
        UseSticker();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;
    }
}
