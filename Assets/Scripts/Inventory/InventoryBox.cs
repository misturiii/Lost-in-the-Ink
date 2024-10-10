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

    public void initialize () {
        bg = GetComponent<Image>();
        initialColor = bg.color;
        inventoryDisplay = GetComponentInParent<InventoryDisplay>();
    }

    public void Select () {
        bg.color = selectColor;
        inventoryDisplay.index = index;
    }

    public void Deselect () {
        bg.color = initialColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryDisplay.PointToInventory(index);
    }
}
