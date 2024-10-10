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


    void Start()
    {
        selected = false;
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
            outline.effectColor = Color.black;
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

    protected abstract void UseStickerBefore ();
    protected abstract void UseStickerAfter ();

    public void Drop(InputAction.CallbackContext context) {
        UseStickerBefore();

        if (InInventory()) {
            if (transform.localPosition.x < -100) {
                inventoryDisplay.AddToSketchBook(this);
                UseStickerAfter();
                Debug.Log($"Sticker moved to Sketchbook: Name = {name}, Index = {index}, Position = {transform.position}");
            } else {
                transform.localPosition = Vector3.zero;
                Debug.Log("Sticker reset to initial position.");
            }
            return;
        }

        UseStickerAfter();
    }

    public bool InInventory() {
        bool inInventory = transform.parent.tag == "Inventory";
        Debug.Log($"Sticker InInventory Check: Name = {name}, InInventory = {inInventory}");
        return inInventory;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Select();
        Debug.Log($"Sticker Begin Drag: Name = {name}, Index = {index}, Position = {transform.position}");
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
        Debug.Log($"Sticker On Drag: Name = {name}, Position = {transform.position}");
    }

    public void Drag(Vector3 move)
    {
        transform.position += move;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (InInventory()) {
            inventoryDisplay.PointToInventory(index);
        } else {
            inventoryDisplay.PointToSketchBook(this);
        }
    }
}
