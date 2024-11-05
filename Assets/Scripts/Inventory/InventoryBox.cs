using System.Diagnostics;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBox : Selectable
{
    public InventoryDisplay inventoryDisplay;
    public int index;
    public Sticker sticker;
    public int count = 0;
    public TextMeshProUGUI countText;

    public void Initialize () {
        inventoryDisplay = GetComponentInParent<InventoryDisplay>();
        countText = GetComponentInChildren<TextMeshProUGUI>();
        countText.text = string.Empty;
    }

    public override void OnSelect (BaseEventData data) {
        base.OnSelect(null);
        inventoryDisplay.SelectInventory(index);
        if (sticker) {
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
        if (sticker) {
            sticker.OnDeselect(null);
        }
    }

    public void RemoveSticker () {
        ItemSticker temp = (ItemSticker)sticker;
        if (sticker.item.count == 0) {
            sticker = null;
            inventoryDisplay.RemoveFromInventory(index);
        }
        OnDeselect(null);
        inventoryDisplay.AddToSketchbook(temp);
    }

    public void SetSticker(Item item) {
        sticker = Instantiate(item.prefab, transform).GetComponent<Sticker>();
        sticker.Initialize(item);
        UpdateCount(item.count);
    }

    public void UpdateCount (int count) {
        countText.text = count > 0 ? count.ToString() : string.Empty;
    }
}
