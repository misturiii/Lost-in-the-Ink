using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class Sticker : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler, IPointerEnterHandler
{
    protected float moveSpeed = 1000f;
    public int index;
    protected InventoryDisplay inventoryDisplay;
    protected InputActions inputActions;
    protected bool selected;
    protected Outline outline;
    protected Canvas canvas;

    public TextMeshProUGUI feedbackText;  
    private readonly float xTolerance = 200f; 
    private readonly float yTolerance = 200f; 
    private Vector3 initialPosition;  

    // 存储每个 Sticker 的目标位置
    private readonly Dictionary<string, List<Vector3>> stickerTargetPositions = new Dictionary<string, List<Vector3>>()
    {
        { "Fountain(Clone)", new List<Vector3> { new Vector3(435.00f, 502.33f, 0.00f) } },
        { "Flower", new List<Vector3> { new Vector3(73.00f, 346.33f, 0.00f), new Vector3(721.00f, 700.33f, 0.00f) } },
        { "Bench(Clone)", new List<Vector3> { new Vector3(330.00f, 300f, 0.00f) } },
        { "IceCream(Clone)", new List<Vector3> { new Vector3(330f, -50f, 0.00f) } },
        { "Tree", new List<Vector3> { new Vector3(916.00f, 83.33f, 0.00f) } },
        { "Ballon", new List<Vector3> { new Vector3(697.00f, 65.33f, 0.00f) } }
    };

    // 追踪每个 Sticker 是否被正确放置
    private readonly Dictionary<string, bool> isPlacedCorrectly = new Dictionary<string, bool>();


    void Start()
    {
        selected = false;
        inventoryDisplay = FindObjectOfType<InventoryDisplay>();
        gameObject.AddComponent<GraphicRaycaster>();
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.size = Vector3.one * 300;
        Initialize();

        initialPosition = transform.position;

        // 初始化 isPlacedCorrectly 字典
        foreach (var stickerName in stickerTargetPositions.Keys)
        {
            if (!isPlacedCorrectly.ContainsKey(stickerName))
            {
                isPlacedCorrectly.Add(stickerName, false);
            }
        }
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
    
        
        initialPosition = transform.position;

        
        feedbackText = GameObject.Find("/Canvas/FeedbackText")?.GetComponent<TextMeshProUGUI>();

        if (feedbackText != null)
        {
            feedbackText.enabled = true;
            feedbackText.text = "";  
            Debug.Log("FeedbackText initialized successfully.");
        }
        else
        {
            Debug.LogError("FeedbackText not found or assigned! Please check the UI setup.");
        }

        Debug.Log($"Sticker Initialized: Name = {name}, Index = {index}, Position = {transform.position}");
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
        Vector3 droppedPosition = transform.position;
        Debug.Log($"Sticker Dropped: Name = {name}, Index = {index}, Position = {droppedPosition}");

        // 检查 Sticker 是否有指定的目标位置
        if (stickerTargetPositions.ContainsKey(name))
        {
            bool isInTargetArea = CheckIfInTargetArea(droppedPosition, stickerTargetPositions[name]);

            if (isInTargetArea)
            {
                ShowFeedbackMessage("Success", 1f);
                isPlacedCorrectly[name] = true; // 设置为已放置正确
                Debug.Log("Success: Sticker placed in the correct area!");
            }
            else
            {
                ShowFeedbackMessage("Try Again", 1f);
                Debug.Log("Try Again: Sticker not placed in the correct area.");
                ReturnToInventory();
            }
        }
        else
        {
            Debug.Log($"Sticker {name} does not have a specified target area.");
        }

        if (InInventory()) {
            if (transform.localPosition.x < -100) {
                inventoryDisplay.AddToSketchBook(this);
                UseStickerAfter();
                Debug.Log($"Sticker moved to Sketchbook: Name = {name}, Index = {index}, Position = {transform.position}");
            } else {
                transform.localPosition = Vector3.zero;
                Debug.Log("Sticker reset to initial position.");
                return;
            }
            return;
        }

        UseStickerAfter();
    }

    private bool CheckIfInTargetArea(Vector3 droppedPosition, List<Vector3> targetPositions)
    {
        foreach (var targetPosition in targetPositions)
        {
            bool isXInRange = Mathf.Abs(droppedPosition.x - targetPosition.x) <= xTolerance;
            bool isYInRange = Mathf.Abs(droppedPosition.y - targetPosition.y) <= yTolerance;

            if (isXInRange && isYInRange)
            {
                return true; // 只要有一个目标区域满足条件，则返回 true
            }
        }

        return false; // 如果所有目标区域都不满足，则返回 false
    }

    private void ReturnToInventory() {
        transform.position = initialPosition;
        Debug.Log($"Sticker returned to initial position: {initialPosition}");
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
    private void ShowFeedbackMessage(string message, float duration) {
        if (feedbackText != null) {
            feedbackText.text = message;
            feedbackText.enabled = true;
            Debug.Log($"[ShowFeedbackMessage] Displaying message: {message}");
            StartCoroutine(HideFeedbackMessage(duration));
        }
        else
        {
            Debug.LogError("[ShowFeedbackMessage] feedbackText is not assigned or found.");
        }
    }

    
    private IEnumerator HideFeedbackMessage(float duration) {
        yield return new WaitForSeconds(duration);
        if (feedbackText != null)
        {
            feedbackText.text = "";
            feedbackText.enabled = false;
            Debug.Log("[HideFeedbackMessage] Feedback message hidden successfully.");
        }
        else
        {
            Debug.LogError("[HideFeedbackMessage] feedbackText is not assigned or found.");
        }
    }
}
