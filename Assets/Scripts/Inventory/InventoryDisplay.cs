using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    Inventory inventory;             // Reference to the inventory
    InventoryBox[] inventoryBoxes;
    InputActions inputActions; 
    public int index = 0;
    public GraphicRaycaster graphicRaycaster;  // Reference to the Canvas' GraphicRaycaster


    void Awake () {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.MovePointer.Enable();
        inputActions.UI.MovePointer.started += MovePointer;
        inputActions.UI.Click.Enable();
        
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
        SelectInventory(index);
    }

    void MovePointer (InputAction.CallbackContext context) {
        Vector2 input = context.ReadValue<Vector2>();
        input.x = input.x > 0 ? 1 : -1;
        input.y = input.y > 0 ? 1 : -1;
        SelectInventory(Mathf.Clamp(index - (int)input.y, 0, inventoryBoxes.Length - 1));
    }

    public void SelectInventory(int i) {
        inventoryBoxes[index].Deselect();
        inventoryBoxes[i].Select();
    }

    public void RemoveSticker (int i) {
        inventory.Remove(i);
    }

    // Method to update the inventory UI when items are added
    public void UpdateInventoryDisplay()
    {
        // Clear previous icons
        if (inventoryBoxes != null) {
            foreach (InventoryBox box in inventoryBoxes)
            {
                if (box.sticker) {
                    Destroy(box.sticker.gameObject);
                    box.sticker = null;
                }
            }
        }

        // Create icons for each item in the inventory
        for (int i = 0; i < inventory.items.Count; i++)
        {
            Item item = inventory.items[i];
            inventoryBoxes[i].sticker = Instantiate(item.prefab, inventoryBoxes[i].transform).GetComponent<Sticker>(); 
        }
    }
}
