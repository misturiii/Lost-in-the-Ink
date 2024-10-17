using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    Inventory inventory;             // Reference to the inventory
    InventoryBox[] inventoryBoxes;
    InputActions inputActions; 
    Sticker[] stickers;
    Transform sketchbook;
    public int index = 0;
    Sticker sketchbookSelect = null;
    bool InInventory = true;
    public GraphicRaycaster graphicRaycaster;  // Reference to the Canvas' GraphicRaycaster


    void Awake () {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.MovePointer.Enable();
        inputActions.UI.MovePointer.started += MovePointer;
        inputActions.UI.Click.Enable();
        inputActions.UI.Switch.performed += Switch;
        inputActions.UI.Click.performed += RefreshSelect;
        
        inventoryBoxes = GetComponentsInChildren<InventoryBox>(true);
        sketchbook = transform.parent.GetChild(0);

        for (int i = 0; i < inventoryBoxes.Length; i++) {
            inventoryBoxes[i].index = i;
            inventoryBoxes[i].initialize();
        }

        stickers = new Sticker[inventoryBoxes.Length];
        inventory = Resources.Load<Inventory>("PlayerInventory");
        graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
    }

    void OnEnable () {
        UpdateInventoryDisplay();
        RefreshSelect(new InputAction.CallbackContext());
    }

    void RefreshSelect (InputAction.CallbackContext context) {
        if (InInventory) {
            SelectInventory(index);
        } else {
            SelectSkechbook(sketchbookSelect);
        }
    }

    void MovePointer (InputAction.CallbackContext context) {
        Vector2 input = context.ReadValue<Vector2>();
        if (InInventory) {
            input.x = input.x > 0 ? 1 : -1;
            input.y = input.y > 0 ? 1 : -1;
            PointToInventory(Mathf.Clamp(index - (int)input.y, 0, inventoryBoxes.Length - 1));
        } else {  
            if (sketchbookSelect) {
                PointToNextSticker(input);
            }
        }
    }

    void PointToNextSticker (Vector2 input) {
        RaycastHit[] hits = Physics.RaycastAll(sketchbookSelect.transform.position, input, 10000000);
        foreach (RaycastHit hit in hits) {
            Debug.Log(hit.collider.name);
            if (hit.collider.tag == "Sticker") {
                Sticker sticker = hit.transform.GetComponent<Sticker>();
                if (!sticker.InInventory()) {
                    PointToSketchBook(sticker);
                }
                return;
            }
        }
    }

    void Switch (InputAction.CallbackContext context) {
        InInventory = !InInventory;
        if (InInventory) {
            PointToInventory(index);
        } else {
            PointToSketchBook(sketchbookSelect);
        } 
    }

    public void DeselectAll () {
        inventoryBoxes[index].Deselect();
        if (stickers[index]) {
            stickers[index].Deselect();
        }
        if (sketchbookSelect) {
            sketchbookSelect.Deselect();
        }
    }

    void SelectInventory(int i) {
        inventoryBoxes[i].Select();
        if (stickers[i]) {
            stickers[i].Select();
        }
    }

    void SelectSkechbook (Sticker sticker) {
        sketchbookSelect = sticker;
        if (sticker) {
            sticker.Select();
        }
    }

    public void PointToSketchBook (Sticker sticker) {
        InInventory = false;
        DeselectAll();
        SelectSkechbook(sticker);
    }

    public void PointToInventory (int i) {
        InInventory = true;
        DeselectAll();
        SelectInventory(i);
    }

    public void AddToSketchBook (Sticker sticker) {   
        sticker.transform.SetParent(sketchbook);
        RemoveSticker(sticker);
        // PointToSketchBook(sticker);
        UpdateInventoryDisplay();
    }

    public void RemoveSticker (Sticker sticker) {
        inventory.Remove(sticker.index);
    }

    public void AddToInventory (ItemSticker sticker, Sticker next) {
        inventory.Add(Resources.Load<Item>(sticker.stickername));
        UpdateInventoryDisplay();
        PointToInventory(inventory.items.Count - 1);
        sketchbookSelect = next;
    }

    // Method to update the inventory UI when items are added
    public void UpdateInventoryDisplay()
    {
        // Clear previous icons
        if (inventoryBoxes != null) {
            foreach (InventoryBox box in inventoryBoxes)
            {
                if (box.transform.childCount == 1) {
                    box.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        Array.Fill(stickers, null);

        // Create icons for each item in the inventory
        for (int i = 0; i < inventory.items.Count; i++)
        {
            Item item = inventory.items[i];
            GameObject sticker = Instantiate(item.prefab, inventoryBoxes[i].transform); 
            stickers[i] = sticker.GetComponent<Sticker>();
            stickers[i].index = i;
            Debug.Log(stickers[i].name);
            Debug.Log(stickers[i].index);
        }
    }
}
