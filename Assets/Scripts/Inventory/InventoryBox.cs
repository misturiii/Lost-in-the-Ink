using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBox : Selectable
{
    public InventoryDisplay inventoryDisplay;
    public int index;
    public Sticker sticker;

    public void initialize () {
        inventoryDisplay = GetComponentInParent<InventoryDisplay>();
    }

    public override void OnSelect (BaseEventData data) {
        base.OnSelect(null);
        inventoryDisplay.inventoryIndex = index;
        if (sticker) {
            sticker.OnSelect(null);
        }
    }

    public override void Select()
    {
        base.Select();
        OnSelect(null);
    }

    public override void OnDeselect (BaseEventData data) {
        base.OnDeselect(null);
        if (sticker) {
            sticker.OnDeselect(null);
        }
    }

    public void RemoveSticker () {
        ItemSticker temp = (ItemSticker)sticker;
        sticker = null;
        OnDeselect(null);
        inventoryDisplay.RemoveFromInventory(index, temp);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    public void SetSticker(GameObject prefab) {
        sticker = Instantiate(prefab, transform).GetComponent<Sticker>();
        sticker.Initialize();
    }
}
