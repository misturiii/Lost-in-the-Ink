using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBox : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] Color selectColor;
    Color initialColor;
    Image bg;
    InventoryDisplay inventoryDisplay;
    public int index;
    public Sticker sticker;

    public void initialize () {
        bg = GetComponent<Image>();
        initialColor = bg.color;
        inventoryDisplay = GetComponentInParent<InventoryDisplay>();
    }

    public void Select () {
        bg.color = selectColor;
        inventoryDisplay.index = index;
        if (sticker) {
            sticker.Select();
        }
    }

    public void Deselect () {
        bg.color = initialColor;
        if (sticker) {
            sticker.Deselect();
        }
    }

    public void RemoveSticker () {
        inventoryDisplay.RemoveSticker(sticker.index);
        sticker = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryDisplay.SelectInventory(index);
    }
}
