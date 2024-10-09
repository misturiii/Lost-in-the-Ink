using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Sticker : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler, IPointerEnterHandler
{
    protected float moveSpeed = 1000f;
    public int index;
    protected InventoryDisplay inventoryDisplay;
    protected InputActions inputActions;
    protected bool selected;
    protected Outline outline;
    protected Canvas canvas;

    void Start () {
        inventoryDisplay = FindObjectOfType<InventoryDisplay>();
        gameObject.AddComponent<GraphicRaycaster>();
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.size = Vector3.one * 300;
        Initialize();
    }

    void OnEnable () {
        if (!canvas) {
            canvas = gameObject.AddComponent<Canvas>();
        } else {
            canvas = gameObject.GetComponent<Canvas>();
        }
        canvas.overrideSorting = true;
        canvas.sortingOrder = 1;
        if (inputActions == null) {
            inputActions = FindObjectOfType<InputActionManager>().inputActions;
            outline = gameObject.AddComponent<Outline>();
            outline.effectColor = Color.white;
            outline.effectDistance = new Vector2(10, -10);
            outline.enabled = false;
        }
    }

    protected abstract void Initialize();

    void Update () {
        if (selected && inputActions.UI.Click.inProgress) {
            if (inputActions.UI.Click.inProgress) {
                Drag(moveSpeed * inputActions.UI.Move.ReadValue<Vector2>() * Time.deltaTime);
            } 
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Drop(new InputAction.CallbackContext());
    }

    public void Drop(InputAction.CallbackContext context) {
        UseStickerBefore();
        if (InInventory()) {
            if (transform.localPosition.x < -100) {
                inventoryDisplay.AddToSketchBook(this);
            } else {
                transform.localPosition = Vector3.zero;
            }
            return;
        }
        UseStickerAfter();
    }

    public bool InInventory() {
        return transform.parent.tag == "Inventory";
    }

    protected abstract void UseStickerBefore ();

    protected abstract void UseStickerAfter();

    public void OnBeginDrag (PointerEventData eventData) {
        Select();
    }

    public void Select() {
        selected = true;
        outline.enabled = true;
        canvas.sortingOrder = 5000;
        inputActions.UI.Click.canceled += Drop;
    }
    
    public void Deselect () {
        selected = false;
        outline.enabled = false;
        canvas.sortingOrder = 1;
        inputActions.UI.Click.canceled -= Drop;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag((Vector3)eventData.delta);
    }

    public void Drag (Vector3 move) {
        transform.position += move;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InInventory()) {
            inventoryDisplay.PointToInventory(index);
        } else {
            inventoryDisplay.PointToSketchBook(this);
        }
    }

}
