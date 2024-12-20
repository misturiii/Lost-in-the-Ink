using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Sticker : Selectable, IDragHandler
{
    protected float moveSpeed = 1000f;
    protected InventoryBox inventoryBox;
    protected InputActions inputActions;
    protected Material material;
    protected SketchbookGuide sketchbookGuide;
    protected Canvas canvas;
    public Item item;
    protected Transform stickerPanel;
    protected bool isSelected;

    static protected readonly int 
        useOutlineId = Shader.PropertyToID("_UseOutline"),
        lineWidthId = Shader.PropertyToID("_LineWidth"),
        lineColorId = Shader.PropertyToID("_LineColor");

    public virtual void Initialize(Item item)
    {
        this.item = item;
        inventoryBox = GetComponentInParent<InventoryBox>();
        inputActions = FindObjectOfType<InputActionManager>().inputActions;

        material = Instantiate(image.material);
        image.material = material;
        
        material.SetInt(useOutlineId, 0);
        material.SetFloat(lineWidthId, 6);

        sketchbookGuide = GameObject.Find("PlayerGuide").GetComponentInChildren<SketchbookGuide>();
        canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 1;
        stickerPanel = GameObject.FindGameObjectWithTag("Sketchbook").transform.GetChild(0);
        gameObject.AddComponent<GraphicRaycaster>();
    }
    void Update () {
        if (isSelected && inputActions.UI.Click.inProgress) {
            Drag(moveSpeed * inputActions.UI.Move.ReadValue<Vector2>() * Time.deltaTime);
        }
    }

    void OnBeginDrag(InputAction.CallbackContext context) {
        if (isSelected) {
            OnBeginDrag();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnBeginDrag();
    }

    void OnBeginDrag () {
        Debug.Log($"Sticker Begin Drag: Name = {name}, Position = {transform.position}");
        Select();
        if (inventoryBox) {
            if (item.count-- > 1) {
                inventoryBox.SetSticker(item);
            } else {
                inventoryBox.UpdateCount(0);
            }
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        Drop(new InputAction.CallbackContext());
    }

    public abstract void Drop(InputAction.CallbackContext context);

    override public void OnSelect(BaseEventData data) {
        isSelected = true;
        if (canvas) {
            canvas.sortingOrder = 50;
        } else {
            transform.SetAsLastSibling();
        }
        base.OnSelect(null);
        material.SetInt(useOutlineId, 1);
        inputActions.UI.Click.canceled += Drop;
        inputActions.UI.Click.performed += OnBeginDrag;
        Debug.Log($"sticker {name} selected");
    }
    
    override public void OnDeselect(BaseEventData data) {
        isSelected = false;
        if (canvas) {
            canvas.sortingOrder = 1;
        }
        base.OnDeselect(null);
        material.SetInt(useOutlineId, 0);
        inputActions.UI.Click.canceled -= Drop;
        inputActions.UI.Click.performed -= OnBeginDrag;
        Debug.Log($"sticker {name} deselected");
    }

    public override void Select()
    {
        Debug.Log("Select");
        base.Select();
        OnSelect(null);
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

    public virtual void Delete() {
        inputActions.UI.Click.canceled -= Drop;
        inputActions.UI.Click.performed -= OnBeginDrag;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected) {
            if (inventoryBox) {
                inventoryBox.Select();
            } else {
                Select();
        }
        }
    }
}
