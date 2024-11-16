using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    Inventory inventory;             // Reference to the inventory
    InventoryBox[] inventoryBoxes;
    InputActions inputActions; 
    List<Selectable> selectables;
    float distanceWeight = 0.5f, moveStep = 0.3f, waitSec = 0.15f;
    public Selectable currentSelected;
    Vector3 input = Vector3.zero;
    bool readNextInput = true;

    void Awake () {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Move.performed += MovePointer;
        inputActions.UI.Move.canceled += CancelMove;
        
        inventoryBoxes = GetComponentsInChildren<InventoryBox>(true);
        selectables = new List<Selectable>();
        inventory = Resources.Load<Inventory>("PlayerInventory");

        for (int i = 0; i < inventoryBoxes.Length; i++) {
            inventoryBoxes[i].Initialize();
            inventoryBoxes[i].index = i;
            if (inventoryBoxes[i].transform.position.y > 0) {
                selectables.Add(inventoryBoxes[i]);
            }
        }
        currentSelected = inventoryBoxes[0];
      
        ItemSticker[] stickerPanel = transform.parent.GetComponentsInChildren<ItemSticker>(true);
        foreach (var sticker in stickerPanel) {
            selectables.Add(sticker);
            sticker.SetUp();
            sticker.SetOutline();
            sticker.item.total++;
            sticker.GenerateObject(false);
        }
    }

    void OnEnable () {
        UpdateInventoryDisplay();
        currentSelected?.Select();
        readNextInput = true;
    }

    public void RemoveFromInventory (int i) {
        inventory.Remove(i);
        OnEnable();
    }

    public void RemoveFromSketchbook (Sticker sticker) {
        selectables.Remove(sticker);
        SelectNearest();
        currentSelected?.Select();
    }

    IEnumerator Wait () {
        readNextInput = false;
        if (input != Vector3.zero) {
            Ray ray = new Ray(currentSelected.transform.position, input);
            SelectNext(ray);
            currentSelected?.Select();
            yield return new WaitForSeconds(waitSec);
        }
        readNextInput = true;
    }

    void MovePointer(InputAction.CallbackContext context) {
        input = context.ReadValue<Vector2>();
        for (int i = 0; i < 2; i++) {
            if (Mathf.Abs(input[i]) < moveStep) {
                input[i] = 0;
            }
        }
        if (readNextInput && !inputActions.UI.Click.inProgress && gameObject.activeInHierarchy) {
            StartCoroutine(Wait());
        }
    }

    void CancelMove (InputAction.CallbackContext context) {
        input = Vector3.zero;
    } 

    void SelectNearest () {
        Vector3 p = currentSelected.transform.position;
        float minDistance = Mathf.Infinity;
        currentSelected = null;
        foreach (var selectable in selectables) {
            if (selectable.gameObject.activeSelf && 
                selectable is not InventoryBox && 
                Vector3.Distance(selectable.transform.position, p) < minDistance) {
                currentSelected = selectable;
            }
        }
        if (!currentSelected) {
            currentSelected = inventoryBoxes[0];
        }
    }

    void SelectNext(Ray ray) {
        float minDistance = Mathf.Infinity;
        foreach (var selectable in selectables) {
            float distance = GetMinDistance(ray, selectable.transform.position);
            if (selectable.gameObject.activeSelf && distance < minDistance) {
                minDistance = distance;
                currentSelected = selectable;
            }
        }
    }

    float GetMinDistance(Ray ray, Vector3 objectPosition)
    {
        // Vector from ray origin to object position
        Vector3 originToObject = objectPosition - ray.origin;

        // Project this vector onto the ray direction to find the closest point
        float projectionLength = Vector3.Dot(originToObject, ray.direction);
        Vector3 closestPoint = ray.origin + ray.direction * projectionLength;

        // Distance between this closest point and the object's position
        return projectionLength > 0 ? 
            Vector3.Distance(closestPoint, objectPosition) + distanceWeight * projectionLength : 
            Mathf.Infinity;
    }

    // Method to update the inventory UI when items are added
    public void UpdateInventoryDisplay()
    {
        // Clear previous icons
        if (inventoryBoxes != null) {
            for (int j = 0; j < inventoryBoxes.Count(); j++)
            {
                Sticker s = inventoryBoxes[j].sticker;
                if (s) {
                    s.Delete();
                    inventoryBoxes[j].sticker = null;
                }
            }
        }

        int i = 0;
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

    public void AddToSketchbook (Selectable sticker) {
        currentSelected = sticker;
        selectables.Add(currentSelected);
    }

    public void AddToInventory(Item item){
        inventory.Add(item);
        UpdateInventoryDisplay();
    }

    public void Select (Selectable selectable) {
        currentSelected = selectable;
    }
}
