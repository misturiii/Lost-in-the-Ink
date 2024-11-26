using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public abstract class Sticker : Selectable, IDragHandler, ICanvasRaycastFilter
{
    protected float moveSpeed = 1000f;
    protected InventoryBox inventoryBox;
    protected InputActionManager inputActionManager;
    protected InputAction clickAction, moveAction;
    protected Material material;
    protected SketchbookGuide sketchbookGuide;
    protected Canvas canvas;
    public Item item;
    protected Transform stickerPanel;
    protected bool isSelected;
    protected Color lineColor, lineColor2;
    protected float lineWidth = 6f;
    protected StickerAudioManager audioManager; 
    [SerializeField] string guide = string.Empty;
    protected GraphicRaycaster raycaster;
    protected Vector3 InitialPositon;
    protected Vector3 pivotOffset;
    protected Texture2D alpha;
    protected float horizontalBound, verticalBound;

    static protected readonly int 
        lineWidthId = Shader.PropertyToID("_LineWidth"),
        lineColorId = Shader.PropertyToID("_LineColor");

    public virtual void Initialize(Item item)
    {
        this.item = item;
        inventoryBox = GetComponentInParent<InventoryBox>();
        canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 25;
        
        gameObject.AddComponent<GraphicRaycaster>();
        SetUp();
    }
    void Update () {
        if (isSelected && clickAction.inProgress) {
            Drag(moveSpeed * moveAction.ReadValue<Vector2>() * Time.deltaTime);
        }
    }

    public void SetUp () {
        isSelected = false;
        material = Instantiate(image.material);
        image.material = material;
        GameObject sketchbook = GameObject.FindWithTag("Sketchbook");
        stickerPanel = sketchbook.transform.GetChild(4);
        audioManager = sketchbook.GetComponent<StickerAudioManager>();
        raycaster = stickerPanel.GetComponent<GraphicRaycaster>();
    
        sketchbookGuide = GameObject.Find("PlayerGuide").GetComponentInChildren<SketchbookGuide>();
        inputActionManager = FindObjectOfType<InputActionManager>();
        clickAction = inputActionManager.inputActions.UI.Click;
        moveAction = inputActionManager.inputActions.UI.Move;

        SetLineColor();
        material.SetFloat(lineWidthId, lineWidth);
        material.SetColor(lineColorId, lineColor2);
        Navigation n = new Navigation();
        n.mode = Navigation.Mode.None;
        navigation = n;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 size = rectTransform.rect.size;

        // Get the pivot offset (relative to the RectTransform's size)
        pivotOffset = new Vector2((0.5f - rectTransform.pivot.x) * size.x, 
                                  (0.5f - rectTransform.pivot.y) * size.y);
        InitialPositon = -pivotOffset;

        horizontalBound = Screen.width - (inventoryBox ? 0 : FunctionLibrary.inventoryWidth);
        verticalBound = Screen.height;
    }

    protected abstract void SetLineColor ();

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
        inputActionManager.SetDragCursor(true);
        Select();
        sketchbookGuide.DisplayDropGuide();
        if (inventoryBox) {
            if (item.count > 1) {
                inventoryBox.MakeCopy(item);
            } else {
                inventoryBox.UpdateCount(0);
            }
            canvas.overrideSorting = true;
            canvas.sortingOrder = 110;
        } else {
            transform.SetAsLastSibling();
        }
        if(!inventoryBox){
            // Play remove audio if not already playing
            audioManager.Play(StickerAudio.remove);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        Drop(new InputAction.CallbackContext());
    }

    public virtual void Drop(InputAction.CallbackContext context) {
        inputActionManager.SetDragCursor(false);
    }

    override public void OnSelect(BaseEventData data) {
        isSelected = true;
        base.OnSelect(null);
        if (!material) {
            Initialize(item);
        }
        material.SetColor(lineColorId, lineColor);
        clickAction.canceled += Drop;
        clickAction.performed += OnBeginDrag;
        sketchbookGuide.toolGuide.text = guide;
        InitialPositon = transform.localPosition;
    }
    
    override public void OnDeselect(BaseEventData data) {
        base.OnDeselect(null);
        isSelected = false;
        if (canvas) {
            canvas.sortingOrder = 25;
        }
        if (!material) {
            Initialize(item);
        }
        material.SetColor(lineColorId, lineColor2);
        clickAction.canceled -= Drop;
        clickAction.performed -= OnBeginDrag;
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

    public void Drag(Vector3 move){
        Vector3 p = transform.position + move + pivotOffset;
        p.x = Mathf.Clamp(p.x, 0, horizontalBound);
        p.y = Mathf.Clamp(p.y, 0, verticalBound);
        transform.position = p - pivotOffset;
    }

    public virtual void Delete() {
        clickAction.canceled -= Drop;
        clickAction.performed -= OnBeginDrag;
        if (inventoryBox && inventoryBox.sticker == this) {
            inventoryBox.sticker = null;
        }
    }

    public override void OnPointerEnter (PointerEventData eventData) {
        if (!isSelected && !eventData.dragging && inputActionManager.isMouse) {
            if (inventoryBox) {
                inventoryBox.Select();
            } else {
                Select();
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

    public Vector3 GetCenterPosition () {
        return transform.position + pivotOffset;
    }

    // Pixel shader for Gaussian blur
    float Sample(int uvx, int uvy, int lineWidth) {
        // Accumulate the color from multiple samples
        float color = 0;
        Texture2D texture = image.sprite.texture;

        // Sample horizontally and vertically
        for (int i = -2; i <= 2; i++) {
            for (int j = -2; j <= 2; j++) {
                int diff = Mathf.Abs(i) - Mathf.Abs(j);
                color += texture.GetPixel(uvx + i * lineWidth, uvy + j * lineWidth).a;
            }
        }
        return color / 25.0f; // Average the horizontal and vertical blur results
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return HasContent(sp, eventCamera);
    }

    bool HasContent (Vector2 screenPoint, Camera eventCamera) {
        Texture2D texture = image.sprite.texture;

        // Convert screen point to local point within the image rect
        RectTransform rectTransform = image.rectTransform;
        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint))
            return false;

        // Convert the local point to texture UV coordinates
        Rect rect = rectTransform.rect;
        float x = (localPoint.x - rect.x) / rect.width;
        float y = (localPoint.y - rect.y) / rect.height;

        // Convert UV coordinates to pixel coordinates
        int pixelX = Mathf.RoundToInt(x * texture.width);
        int pixelY = Mathf.RoundToInt(y * texture.height);

        if (pixelX < 0 || pixelX >= texture.width || pixelY < 0 || pixelY >= texture.height)
            return false;

        // Get the pixel color and check the alpha value
        return Sample(pixelX, pixelY, (int)Mathf.Ceil(lineWidth * 5)) > 0;
    }
}
