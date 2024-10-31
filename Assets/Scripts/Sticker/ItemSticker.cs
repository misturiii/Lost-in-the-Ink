using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemSticker : Sticker
{
    [SerializeField] protected GameObject prefab;
    InventoryDisplay inventoryDisplay = null;
    Transform sketchbookPanel;
    Transform item = null;
    public float rototation = 0;
    public int index;
    
    public override void Initialize() {
        base.Initialize();
        sketchbookPanel = GameObject.FindWithTag("Sketchbook").transform.GetChild(0);
        enabled = true;
        SetNavigationMode(Navigation.Mode.None);
    }

    private void SetNavigationMode (Navigation.Mode mode) {
        Navigation nav = navigation;
        nav.mode = mode;
        navigation = nav;
    }

    public override void Drop(InputAction.CallbackContext context) {
        Destroy(GetComponent<GraphicRaycaster>());
        Destroy(GetComponent<Canvas>());
        if (inventoryBox && transform.localPosition.x >= -100) {
            transform.localPosition = Vector3.zero;
        } else {
            if (!item) {
                inventoryDisplay = inventoryBox.inventoryDisplay;
                transform.SetParent(sketchbookPanel);
                item = Instantiate(prefab).transform;
                SetNavigationMode(Navigation.Mode.Automatic);
                inventoryBox.RemoveSticker();
                inventoryBox = null;
            }
            item.localPosition = TransformationFunction.BookToWorld(transform.localPosition);
            item.localEulerAngles = new Vector3(0, rototation, 0);
        }
    }

    public override void Delete()
    {
        base.Delete();
        if (item) {
            Destroy(item.gameObject);
            inventoryDisplay.RemoveFromSketchbook(this);  
        }
        Destroy(gameObject); 
    }
}
