using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro; 

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

    private readonly Vector3 targetPosition = new Vector3(435.00f, 502.33f, 0.00f); 
    private readonly float xTolerance = 200f; 
    private readonly float yTolerance = 200f; 
    private Vector3 initialPosition;  
    void Start()
    {
        selected = false;
        sketchbook = GameObject.FindGameObjectWithTag("Sketchbook").transform;
        inventory = Resources.Load<Inventory>("PlayerInventory");
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
    }    }

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

        
        Vector3 droppedPosition = transform.position;
        Debug.Log($"Sticker Dropped: Name = {name}, Index = {index}, Position = {droppedPosition}");

        
        if (name.Contains("Fountain"))
        {
            
            bool isInTargetArea = CheckIfInTargetArea(droppedPosition);

            if (isInTargetArea)
            {
                ShowFeedbackMessage("Success", 1f);
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
            Debug.Log($"Sticker Name is not Fountain, skipping position check: Name = {name}");
        }

        if (InInventory())
        {
            if (transform.localPosition.x < -100)
            {
                transform.SetParent(sketchbook);
                inventory.Remove(index);
                inventoryDisplay.pointToSketchBook();
                inventoryDisplay.UpdateInventoryDisplay();
                Debug.Log($"Sticker moved to Sketchbook: Name = {name}, Index = {index}, Position = {transform.position}");
            }
            else
            {
        if (InInventory()) {
            if (transform.localPosition.x < -100) {
                inventoryDisplay.AddToSketchBook(this);
            } else {
                transform.localPosition = Vector3.zero;
                Debug.Log("Sticker reset to initial position.");
                return;
            }
            return;
        }

        UseStickerAfter();
    }

        private bool CheckIfInTargetArea(Vector3 droppedPosition)
    {
        
        bool isXInRange = Mathf.Abs(droppedPosition.x - targetPosition.x) <= xTolerance;
        
        bool isYInRange = Mathf.Abs(droppedPosition.y - targetPosition.y) <= yTolerance;

        
        return isXInRange && isYInRange;
    }

        private void ReturnToInventory()
    {
        
        transform.position = initialPosition;
        Debug.Log($"Sticker returned to initial position: {initialPosition}");
    }

    protected bool InInventory()
    {
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InInventory()) {
            inventoryDisplay.PointToInventory(index);
        } else {
            inventoryDisplay.PointToSketchBook(this);
        }
    }

}

        Debug.Log($"Sticker Moving: Name = {name}, New Position = {transform.position}");
    }

        private void ShowFeedbackMessage(string message, float duration)
    {
        if (feedbackText != null)
        {
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

    
    private IEnumerator HideFeedbackMessage(float duration)
    {
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

    protected abstract void UseStickerBefore();
    protected abstract void UseStickerAfter();
}