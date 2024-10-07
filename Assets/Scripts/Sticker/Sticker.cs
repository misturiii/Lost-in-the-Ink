using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Sticker : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler
{
    protected float moveSpeed = 1000f;
    protected Transform sketchbook;
    public int index;
    protected Inventory inventory;
    protected InventoryDisplay inventoryDisplay;
    protected Canvas canvas;
    protected InputActions inputActions;
    protected bool selected;

    void Start () {
        selected = false;
        sketchbook = GameObject.FindGameObjectWithTag("Sketchbook").transform;
        inventory = Resources.Load<Inventory>("PlayerInventory");
        inventoryDisplay = FindObjectOfType<InventoryDisplay>();
        canvas = gameObject.AddComponent<Canvas>();
        gameObject.AddComponent<GraphicRaycaster>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 1;
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        Initialize();
    }


    protected abstract void Initialize();

    void Update () {
        canvas = gameObject.GetComponent<Canvas>();
        if (selected) {
            if (inputActions.UI.Click.inProgress) {
                Drag(moveSpeed * inputActions.UI.Move.ReadValue<Vector2>() * Time.deltaTime);
            } 
        }
    }

    public void EnableInputAction() {
        selected = true;
        Select();
        inputActions.UI.Click.canceled += Drop;
    }

    public void DisableInputAction() {
        selected = false;
        inputActions.UI.Click.canceled -= Drop;
        Drop(new InputAction.CallbackContext());
    }

    public void OnDrop(PointerEventData eventData)
    {
        Drop(new InputAction.CallbackContext());
    }

    public void Drop(InputAction.CallbackContext context) {
        canvas.sortingOrder = 1;
        UseStickerBefore();
        if (InInventory()) {
            if (transform.localPosition.x < -100) {
                transform.SetParent(sketchbook);
                inventory.Remove(index);
                inventoryDisplay.pointToSketchBook();
                inventoryDisplay.UpdateInventoryDisplay();
            } else {
                transform.localPosition = Vector3.zero;
                return;
            }
        }
        UseStickerAfter();
    }

    protected bool InInventory() {
        return transform.parent.tag == "Inventory";
    }

    protected abstract void UseStickerBefore ();

    protected abstract void UseStickerAfter();

    public void OnBeginDrag (PointerEventData eventData) {
        Select();
    }

    public void Select() {
        canvas.sortingOrder = 5000;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag((Vector3)eventData.delta);
    }

    public void Drag (Vector3 move) {
        transform.position += move;
    }
}
