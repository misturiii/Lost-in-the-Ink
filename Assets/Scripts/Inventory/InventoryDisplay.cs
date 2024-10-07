using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryDisplay : MonoBehaviour
{
    Inventory inventory;             // Reference to the inventory
    InventoryBox[] inventoryBoxes;
    InputActions inputActions; 
    Sticker[] stickers;
    Sticker prev;
    public int index;
    bool pointToInventory;

    void Awake () {
        index = 0;
        prev = null;
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Toggle.Enable();
        inputActions.UI.Toggle.started += Toggle;
        inputActions.UI.Click.Enable();
        inputActions.UI.Click.performed += Click;
        inputActions.UI.Switch.performed += Switch;
        
        inventoryBoxes = GetComponentsInChildren<InventoryBox>(true);
        for (int i = 0; i < inventoryBoxes.Length; i++) {
            inventoryBoxes[i].index = i;
            inventoryBoxes[i].initialize();
        }
        stickers = new Sticker[inventoryBoxes.Length];
        inventory = Resources.Load<Inventory>("PlayerInventory");

        ResetSelect(0);
    }

    public void ResetSelect(int i) {
        Select(i);
        pointToInventory = true;
    }

    void OnEnable () {
        UpdateInventoryDisplay();
    }

    void Toggle (InputAction.CallbackContext context) {
        if (pointToInventory) {
            int diff = context.ReadValue<float>() > 0 ? 1 : -1;
            Select(Mathf.Clamp(index - diff, 0, inventoryBoxes.Length - 1));
        }
    }

    void Click (InputAction.CallbackContext context) {
        if (pointToInventory) {
            if (prev) {
                    prev.DisableInputAction();
                }
            if (stickers[index]) {
                stickers[index].EnableInputAction();
                prev = stickers[index];
            }
        } else {
            prev.EnableInputAction();
        }
    }

    void Switch (InputAction.CallbackContext context) {
        pointToInventory = !pointToInventory;
        if (!pointToInventory) {
            if (prev) {
                prev.DisableInputAction();
            }
            inventoryBoxes[index].Deselect();
        } else {
            inventoryBoxes[index].Select();
        }
    }

    public void pointToSketchBook () {
        pointToInventory = false;
        inventoryBoxes[index].Deselect();
    }

    public void Select(int i) {
        inventoryBoxes[index].Deselect();
        inventoryBoxes[i].Select();
    }

    // Method to update the inventory UI when items are added
    public void UpdateInventoryDisplay()
    {
        // Clear previous icons
        if (inventoryBoxes != null) {
            foreach (InventoryBox box in inventoryBoxes)
            {
                if (box.transform.childCount == 1) {
                    Destroy(box.transform.GetChild(0).gameObject);
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
        }
    }
}
