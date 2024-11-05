using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    Inventory inventory;             // Reference to the inventory
    InventoryBox[] inventoryBoxes;
    InputActions inputActions; 
    int inventoryIndex = 0;
    Selectable sketchbookSelect = null;
    public GraphicRaycaster graphicRaycaster;  // Reference to the Canvas' GraphicRaycaster
    bool inInventory = true;
    List<Selectable> stickers;
    int numTools = 0;



    void Awake () {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.MovePointer.Enable();
        inputActions.UI.Click.Enable();
        inputActions.UI.Switch.performed += Switch;
        
        inventoryBoxes = GetComponentsInChildren<InventoryBox>(true);

        for (int i = 0; i < inventoryBoxes.Length; i++) {
            inventoryBoxes[i].index = i;
            inventoryBoxes[i].Initialize();
        }

        inventory = Resources.Load<Inventory>("PlayerInventory");
        graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
        stickers = new List<Selectable>();

        ItemSticker[] stickerPanel = transform.parent.GetChild(transform.parent.childCount - 1).GetComponentsInChildren<ItemSticker>(true);
        foreach (var sticker in stickerPanel) {
            stickers.Add(sticker);
            sticker.SetUp();
            sticker.item.total++;
            sketchbookSelect = sticker;
            sticker.ResetObject();
        }
    }

    void OnEnable () {
        UpdateInventoryDisplay();
        Select();
    }

    void Select () {
        if (!inInventory && sketchbookSelect) {
            sketchbookSelect.Select();
        } else {
            inventoryBoxes[inventoryIndex].Select();
            inInventory = true;
        }
    }

    void Switch (InputAction.CallbackContext context) {
        inInventory = !inInventory;
        Select();
    }

    public void RemoveFromInventory (int i) {
        inventory.Remove(i - 3);
        OnEnable();
    }

    public void AddToSketchbook (ItemSticker sticker) {
        inInventory = false;
        sketchbookSelect = sticker;
        stickers.Add(sticker);
    }

    public void RemoveFromSketchbook (Sticker sticker) {
        sketchbookSelect = FindNext(sticker.navigation);
        stickers.Remove(sticker);
        Destroy(sticker.gameObject);
        Select();
    }

    Selectable FindNext(Navigation navigation) {
        Selectable next = null;
        if (next = navigation.selectOnUp) { return next; }
        if (next = navigation.selectOnLeft) { return next; }
        if (next = navigation.selectOnDown) { return next; }
        if (next = navigation.selectOnRight) { return next; }
        return stickers.Count > 0 ? stickers[stickers.Count - 1] : null;
    }

    // Method to update the inventory UI when items are added
    public void UpdateInventoryDisplay()
    {
        // Clear previous icons
        if (inventoryBoxes != null) {
            for (int j = numTools; j < inventoryBoxes.Count(); j++)
            {
                Sticker s = inventoryBoxes[j].sticker;
                if (s) {
                    s.Delete();
                    inventoryBoxes[j].sticker = null;
                }
            }
        }

        int i = numTools;
        while (i < inventory.tools.Count)
        {
            inventoryBoxes[i].SetSticker(inventory.tools[i++]);
        }
        numTools = i;
        i = 3;
        // Create icons for each item in the inventory
        foreach (Item item in inventory.items)
        {
            if (i < inventoryBoxes.Length) {
                inventoryBoxes[i++].SetSticker(item);
            }
        }
        if (i < inventoryBoxes.Length) {
            inventoryBoxes[i].UpdateCount(0);
        }
    }

    public void InventoryAdd(Item item){
        inventory.Add(item);
        UpdateInventoryDisplay();
    }

    public void SelectSketchbook (Selectable selectable) {
        sketchbookSelect = selectable;
        inInventory = false;
    }

    public void SelectInventory (int i) {
        inventoryIndex = i;
        inInventory = true;
    }
}
