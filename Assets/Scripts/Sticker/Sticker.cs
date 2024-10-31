using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public abstract class Sticker : Selectable, IDragHandler, IDropHandler, IBeginDragHandler
{
    protected float moveSpeed = 1000f;
    protected InventoryBox inventoryBox;
    protected InputActions inputActions;
    protected Material material;
    protected SketchbookGuide sketchbookGuide;

    static protected readonly int 
        useOutlineId = Shader.PropertyToID("_UseOutline"),
        lineWidthId = Shader.PropertyToID("_LineWidth"),
        lineColorId = Shader.PropertyToID("_LineColor");

    public virtual void Initialize()
    {
        inventoryBox = GetComponentInParent<InventoryBox>();
        gameObject.AddComponent<GraphicRaycaster>();
        inputActions = FindObjectOfType<InputActionManager>().inputActions;

        material = Instantiate(image.material);
        image.material = material;
        
        material.SetInt(useOutlineId, 0);
        material.SetFloat(lineWidthId, 6);

        sketchbookGuide = GameObject.Find("PlayerGuide").GetComponentInChildren<SketchbookGuide>();
    }
    void Update () {
        if (currentSelectionState == SelectionState.Selected && inputActions.UI.Click.inProgress) {
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
        Debug.Log($"Sticker Begin Drag: Name = {name}, Position = {transform.position}");
    }

    override public void OnSelect(BaseEventData data) {
        base.OnSelect(null);
        material.SetInt(useOutlineId, 1);
        inputActions.UI.Click.canceled += Drop;
        Debug.Log($"sticker {name} selected");
    }
    
    override public void OnDeselect (BaseEventData data) {
        base.OnDeselect(null);
        material.SetInt(useOutlineId, 0);
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

    override public void OnPointerEnter(PointerEventData eventData) {
        if (inventoryBox) {
            inventoryBox.Select();
        } else {
            Select();
        }
    }

    public virtual void Delete() {
        inputActions.UI.Click.canceled -= Drop;
    }
}
