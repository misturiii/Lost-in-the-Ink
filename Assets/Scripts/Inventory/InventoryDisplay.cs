using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    Inventory inventory;             // Reference to the inventory
    InventoryBox[] inventoryBoxes;
    InputActions inputActions; 
    public int inventoryIndex = 0;
    Selectable sketchbookSelect = null;
    public GraphicRaycaster graphicRaycaster;  // Reference to the Canvas' GraphicRaycaster
    bool inInventory = true;


    void Awake () {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.MovePointer.Enable();
        inputActions.UI.Click.Enable();
        inputActions.UI.Switch.performed += Switch;
        
        inventoryBoxes = GetComponentsInChildren<InventoryBox>(true);

        for (int i = 0; i < inventoryBoxes.Length; i++) {
            inventoryBoxes[i].index = i;
            inventoryBoxes[i].initialize();
        }

        inventory = Resources.Load<Inventory>("PlayerInventory");
        graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
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

    public void RemoveFromInventory (int i, ItemSticker sticker) {
        inventory.Remove(i);
        inInventory = false;
        sketchbookSelect = sticker;
        OnEnable();
    }

    public void RemoveFromSketchbook (Sticker sticker) {
        sketchbookSelect = FindNext(sticker.navigation);
        Destroy(sticker.gameObject);
        Select();
    }

    Selectable FindNext(Navigation navigation) {
        Selectable next = null;
        if (next = navigation.selectOnUp) { return next; }
        if (next = navigation.selectOnLeft) { return next; }
        if (next = navigation.selectOnDown) { return next; }
        if (next = navigation.selectOnRight) { return next; }
        return null;
    }

    // Method to update the inventory UI when items are added
    public void UpdateInventoryDisplay()
    {
        // Clear previous icons
        if (inventoryBoxes != null) {
            foreach (InventoryBox box in inventoryBoxes)
            {
                if (box.sticker) {
                    box.sticker.Delete();
                    box.sticker = null;
                }
            }
        }

        // Create icons for each item in the inventory
        for (int i = 0; i < inventory.items.Count; i++)
        {
            Item item = inventory.items[i];
            inventoryBoxes[i].SetSticker(item.prefab);
        }
    }
}
