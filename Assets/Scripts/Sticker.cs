using UnityEngine;
using UnityEngine.EventSystems;

public class Sticker : MonoBehaviour, IDragHandler, IDropHandler
{
    [SerializeField, Min(0)] float initialHeight = 5;
    [SerializeField, Min(0)] float sketchbookWidth = 40, sketchbookHeight = 30;  
    [SerializeField] GameObject prefab;
    Transform copy;

    void Start () {
        copy = null;
    }

    public void UseSticker() {
        float x = -transform.localPosition.x / Screen.width * sketchbookWidth;
        float z = -transform.localPosition.y / Screen.height * sketchbookHeight;
        if (!copy) {
            copy = Instantiate(prefab).transform;
        }
        copy.localPosition = new Vector3(x, initialHeight, z);
        copy.localEulerAngles = Vector3.zero;
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
