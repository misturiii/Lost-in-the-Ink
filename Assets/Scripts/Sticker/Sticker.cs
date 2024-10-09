using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro; 

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
        canvas = gameObject.AddComponent<Canvas>();
        gameObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 1;
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        Initialize();

        
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

    protected abstract void Initialize();

    void Update()
    {
        canvas = gameObject.GetComponent<Canvas>();
        if (selected)
        {
            if (inputActions.UI.Click.inProgress)
            {
                Drag(moveSpeed * inputActions.UI.Move.ReadValue<Vector2>() * Time.deltaTime);
            }
        }
    }

    public void EnableInputAction()
    {
        selected = true;
        Select();
        Debug.Log($"Sticker Enabled: Name = {name}, Index = {index}, Position = {transform.position}");
        inputActions.UI.Click.canceled += Drop;
    }

    public void DisableInputAction()
    {
        selected = false;
        Debug.Log($"Sticker Disabled: Name = {name}, Index = {index}, Position = {transform.position}");
        inputActions.UI.Click.canceled -= Drop;
        Drop(new InputAction.CallbackContext());
    }

    public void OnDrop(PointerEventData eventData)
    {
        Drop(new InputAction.CallbackContext());
    }

    public void Drop(InputAction.CallbackContext context)
    {
        canvas.sortingOrder = 1;
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
                transform.localPosition = Vector3.zero;
                Debug.Log("Sticker reset to initial position.");
                return;
            }
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

    public void Select()
    {
        canvas.sortingOrder = 5000;
        Debug.Log($"Sticker Selected: Name = {name}, Index = {index}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag((Vector3)eventData.delta);
        Debug.Log($"Sticker On Drag: Name = {name}, Position = {transform.position}");
    }

    public void Drag(Vector3 move)
    {
        transform.position += move;
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