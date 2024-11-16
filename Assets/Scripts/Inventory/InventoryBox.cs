using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBox : Selectable
{
    public InventoryDisplay inventoryDisplay;
    public int index;
    public Sticker sticker = null, next = null;
    public int count = 0;
    public TextMeshProUGUI countText;

    public void Initialize () {
        inventoryDisplay = GetComponentInParent<InventoryDisplay>();
        countText = GetComponentInChildren<TextMeshProUGUI>();
        countText.text = string.Empty;
    }

    public override void OnSelect (BaseEventData data) {
        base.OnSelect(data);
        inventoryDisplay.Select(this);
        if (next) {
            next.OnSelect(data);
            sticker = next;
            next = null;
        } else if (sticker) {
            sticker.OnSelect(data);
        }
    }

    public override void Select()
    {
        base.Select();
        OnSelect(null);
    }

    public override void OnDeselect (BaseEventData data) {
        base.OnDeselect(null);
        sticker?.OnDeselect(null);
    }

    public void RemoveSticker () {
        ItemSticker temp = null;
        bool canRemove = false;
        if (sticker is ItemSticker) {
            canRemove = ((ItemSticker)sticker).item.count <= 1;
            temp = (ItemSticker)sticker;
        } else if (sticker is PieceSticker) {
            canRemove = true;
            temp = ((PieceSticker)sticker).newItemSticker;
        }
        inventoryDisplay.AddToSketchbook(temp);
        Debug.Log("Add sticker to Sketchbook");
        sticker = next;
        next = null;
        if (canRemove) {
            inventoryDisplay.RemoveFromInventory(index);
        }
    }

    public void SetSticker(Item item) {
        sticker = Instantiate(item.prefab, transform).GetComponent<Sticker>();
        sticker.Initialize(item);
        UpdateCount(item.count);
    }

    public void MakeCopy(Item item) {
        next = Instantiate(item.prefab, transform).GetComponent<Sticker>();
        next.Initialize(item);
        UpdateCount(item.count - 1);
    }

    public void UpdateCount (int count) {
        countText.text = count > 0 ? count.ToString() : string.Empty;
    }

    public override void OnPointerEnter (PointerEventData eventData) {
        Select();
    }
}
