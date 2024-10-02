using UnityEngine;
using UnityEngine.EventSystems;

public class Sticker : MonoBehaviour, IDragHandler, IDropHandler
{
    [SerializeField, Min(0)] float initialHeight = 5;
    [SerializeField, Min(0)] float sketchbookWidth = 40, sketchbookHeight = 30;  
    [SerializeField] float inventoryWidth = 400;
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject airwall;
    Transform copy;

    void Start () {
        copy = null;
        // Find the GameObject with the tag "airWall" and assign it to airwall
        airwall = GameObject.FindWithTag("airWall");

        // Check if the airwall was found
        if (airwall == null)
        {
            Debug.LogError("Airwall object with tag 'airWall' not found in the scene!");
        }
        
    }

    public void UseSticker() {
        float x = (-transform.position.x) / (Screen.width - inventoryWidth) * sketchbookWidth;
        float z = -transform.position.y / Screen.height * sketchbookHeight;
        if (!copy) {
            copy = Instantiate(prefab).transform;
        }
        copy.localPosition = new Vector3(x, initialHeight, z);
        copy.localEulerAngles = Vector3.zero;
    }

    public void OnDrop(PointerEventData eventData)
    {
        UseSticker();
        CheckAndDisableAirwall();

        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;

    }

    private void CheckAndDisableAirwall()
    {
        if (gameObject.CompareTag("key")) // Assuming the sticker is tagged as "Key"
        {
            if (airwall != null && airwall.activeSelf)
            {
                airwall.SetActive(false); // Disable the airwall
                Debug.Log("Airwall disabled because the 'Key' sticker was used.");
            }
        }
    }

}
