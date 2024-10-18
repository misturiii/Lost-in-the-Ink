using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Sticker : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler, IPointerEnterHandler
{
    protected float moveSpeed = 1000f;
    public int index;
    protected InventoryBox inventoryBox;
    protected InputActions inputActions;
    protected bool selected;
    protected Canvas canvas;
    protected SketchbookGuide sketchbookGuide;


    void Start()
    {
        inventoryBox = GetComponentInParent<InventoryBox>();
        gameObject.AddComponent<GraphicRaycaster>();

        Outline outline = gameObject.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = Vector2.one * 2;
        sketchbookGuide = GameObject.Find("PlayerGuide").GetComponentInChildren<SketchbookGuide>();
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

    public abstract void Drop(InputAction.CallbackContext context);

    public void OnBeginDrag(PointerEventData eventData)
    {
        Select();
        Debug.Log($"Sticker Begin Drag: Name = {name}, Index = {index}, Position = {transform.position}");
    }

    public void Select() {
        selected = true;
        canvas.sortingOrder = 5000;
        inputActions.UI.Click.canceled += Drop;
        Debug.Log($"sticker {name} selected");
    }
    
    public void Deselect () {
        selected = false;
        canvas.sortingOrder = 1;
        inputActions.UI.Click.canceled -= Drop;
        Debug.Log($"sticker {name} deselected");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag((Vector3)eventData.delta);
        Debug.Log($"Sticker On Drag: Name = {name}, Position = {transform.position}");
    }

    public void Drag(Vector3 move)
    {
        transform.position += move;
        sketchbookGuide.DisplayDropGuide();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        inventoryBox.OnPointerEnter(eventData);
    }
}
