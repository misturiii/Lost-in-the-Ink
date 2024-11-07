using System;
using System.Collections.Generic;
using TMPro;
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
    protected Color lineColor;
    protected float lineWidth = 6f;
    public AudioClip removeAudioClip; // The audio clip to play when inventory box is removed
    public AudioSource audioSource; 
    [SerializeField] string guide = string.Empty;
    protected GraphicRaycaster raycaster;
    protected Vector3 InitialPositon;

    static protected readonly int 
        lineWidthId = Shader.PropertyToID("_LineWidth"),
        lineColorId = Shader.PropertyToID("_LineColor");

    public virtual void Initialize(Item item)
    {
        this.item = item;
        inventoryBox = GetComponentInParent<InventoryBox>();
        canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 1;
        
        gameObject.AddComponent<GraphicRaycaster>();
        SetUp();
        
    }
    void Update () {
        if (isSelected && inputActions.UI.Click.inProgress) {
            Drag(moveSpeed * inputActions.UI.Move.ReadValue<Vector2>() * Time.deltaTime);
        }
    }

    public void SetUp () {
        isSelected = false;
        material = Instantiate(image.material);
        image.material = material;
        stickerPanel = GameObject.FindWithTag("Sketchbook").transform.GetChild(4);
        raycaster = stickerPanel.GetComponent<GraphicRaycaster>();
        audioSource = GetComponent<AudioSource>();
    
        material.SetFloat(lineWidthId, lineWidth);
        material.SetColor(lineColorId, Color.white);
        sketchbookGuide = GameObject.Find("PlayerGuide").GetComponentInChildren<SketchbookGuide>();
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        lineColor = FunctionLibrary.LineColor2;
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
        sketchbookGuide.DisplayDropGuide();
        if (inventoryBox) {
            item.count--;
            if (item.count > 0) {
                inventoryBox.SetSticker(item);
            } else {
                inventoryBox.UpdateCount(0);
            }
            canvas.sortingOrder = 20;
        } else {
            transform.SetAsLastSibling();
        }
        if(!inventoryBox){
            // Play remove audio if not already playing
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(removeAudioClip);
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
        base.OnSelect(null);
        if (!material) {
            Initialize(item);
        }
        material.SetColor(lineColorId, lineColor);
        inputActions.UI.Click.canceled += Drop;
        inputActions.UI.Click.performed += OnBeginDrag;
        sketchbookGuide.toolGuide.text = guide;
        InitialPositon = transform.localPosition;
    }
    
    override public void OnDeselect(BaseEventData data) {
        isSelected = false;
        if (canvas) {
            canvas.sortingOrder = 1;
        }
        base.OnDeselect(null);
        if (!material) {
            Initialize(item);
        }
        material.SetColor(lineColorId, Color.white);
        inputActions.UI.Click.canceled -= Drop;
        inputActions.UI.Click.performed -= OnBeginDrag;
        Debug.Log($"sticker {name} deselected");
        sketchbookGuide.toolGuide.text = string.Empty;
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
        // Debug.Log($"Sticker On Drag: Name = {name}, Position = {transform.position}");
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
            }
        }
    }

    public List<RaycastResult> DetectOverlap() {
        PointerEventData data = new PointerEventData(EventSystem.current) { position = transform.position};
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        Debug.Log("Drop position" + data.position);
        return results;
    }
}
