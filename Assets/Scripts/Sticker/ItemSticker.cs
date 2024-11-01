using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemSticker : Sticker
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] float scale = 1;
    [SerializeField] InventoryDisplay inventoryDisplay = null;
    [SerializeField] Transform copy = null;
    public float rototation = 0;
    public int index;
    
    public override void Initialize(Item item) {
        base.Initialize(item);
        enabled = true;
        lineColor = FunctionLibrary.LineColor2;
    }

    private void SetNavigationMode (Navigation.Mode mode) {
        Navigation nav = navigation;
        nav.mode = mode;
        navigation = nav;
    }

    public override void Drop(InputAction.CallbackContext context) {
        if (inventoryBox && transform.localPosition.x >= -100) {
            inventoryBox.UpdateCount(++item.count);
            inventoryBox.Select();
            if (item.count == 1) {
                transform.localPosition = Vector3.zero;    
            } else {
                Delete();
            }
        } else {
            if (!copy) {
                inventoryDisplay = inventoryBox.inventoryDisplay;
                transform.SetParent(stickerPanel);
                copy = Instantiate(prefab, GameObject.FindWithTag("World").transform).transform;
                copy.AddComponent<ColorChange>().itemName = item.itemName;
                SetNavigationMode(Navigation.Mode.Automatic);
                inventoryBox.RemoveSticker();
                inventoryBox = null;
                transform.localScale *= scale;
                material.SetFloat(lineWidthId, lineWidth / scale);
            }
            if (canvas) {
                Destroy(GetComponent<GraphicRaycaster>());
                Destroy(GetComponent<Canvas>());
                canvas = null;
            }
            GenerateObject();
            Select();
        }
    }

    public override void OnSelect(BaseEventData data)
    {
        base.OnSelect(null);
        if (inventoryDisplay) {
            inventoryDisplay.SelectSketchbook(this);
        }
    }

    public override void Delete()
    {
        base.Delete();
        if (copy) {
            Destroy(copy.gameObject);
            inventoryDisplay.RemoveFromSketchbook(this);  
        }
        Destroy(gameObject); 
    }

    public bool TrashCan() {
        if (item.total <= 1) {
            return false;
        } else {
            item.total -= 1;
            Delete();
            return true;
        }
    }

    public bool Camera() {
        if (item.total == 9) {
            return false;
        } else {
            inventoryDisplay.InventoryAdd(item);
            return true;
        }
    }

    public bool Mirror(){

         if (item != null)
        {
            transform.Rotate(0, 0, 45);
            rototation -= 45;
            GenerateObject();
            return true;
        }else{
            return false;
        }

    }

    public void GenerateObject(){
        copy.localPosition = FunctionLibrary.BookToWorld(transform.localPosition);
        copy.localEulerAngles = new Vector3(0, rototation, 0);
        copy.GetComponent<ColorChange>().Reset();
    }
}
