using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI; // For Image
using TMPro; // For TextMeshPro

public class PickupObject : MonoBehaviour
{
    Inventory inventory;                // Reference to the Inventory ScriptableObject
    public InventoryDisplay inventoryDisplay;  // Reference to the InventoryDisplay
    private ItemObject currentItem;      // Track the currently detected item
    InputActions inputActions;
    public GameObject pickUpGuide;
    float rayDistance = 3f;  // Distance the ray can detect
    float minDistance = 3f;
    public AudioSource audioSource;
    public AudioClip ToolStickerClip;
    public AudioClip ItemStickerClip;
    private StickerTaskbar stickerTaskbar;
    ToolManager toolManager;

    public delegate void PickBehaviour();
    public PickBehaviour startPick;
    public PickBehaviour endPick;
    void Start()
    {
        inventory = Resources.Load<Inventory>("PlayerInventory");
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.GrabSticker.Enable();
        inputActions.Player.GrabSticker.performed += Grab;

        // Initially hide the pickup text and background
        pickUpGuide.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        stickerTaskbar = FindObjectOfType<StickerTaskbar>();
        toolManager = GetComponent<ToolManager>();
    }

     void Update()
    {
        // Cast a ray from the player's camera forward
        RaycastFromCamera();
    }

     void RaycastFromCamera()
    {
        // Cast a ray from the camera's position forward, I placed it 0,0,0
        Camera mainCamera = Camera.main;
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);  // Ray cast from the camera's position forward
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red,1f);


         if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Check if the object has the Pickable tag
            if (hit.collider.CompareTag("PickableItem"))
            {   
                UpdateCurrentItem(hit.collider.gameObject.GetComponent<ItemObject>());      
                ShowPickupGuide();          
            }
            else{
                HidePickupGuide();// Do not show guide
            }
        }
        else{
            HidePickupGuide();
        }
    }

    void UpdateCurrentItem(ItemObject other) {
        currentItem?.Exit();
        currentItem = other;
        currentItem?.Enter();
    }

    void Grab(InputAction.CallbackContext context)
    {   
        // Check for the E key press and if there's a current item
        if (context.performed && currentItem != null)
        {
            if (CheckPosition()) {
                Debug.Log("Picked up item: " + currentItem.item.itemName);
                if(currentItem.item.isTool){
                    audioSource.PlayOneShot(ToolStickerClip);
                    toolManager.AddTool(currentItem.item.toolType);
                }else{
                    audioSource.PlayOneShot(ItemStickerClip);
                    inventory.Add(currentItem.item); // Add the item to the inventory
                    stickerTaskbar.AddSticker();
                }

                // Destroy the item from the scene
                Destroy(currentItem.gameObject);
                currentItem = null; // Reset current item

                // Hide the pickup text and background after pickup
                HidePickupGuide();
                
            } else {
                Debug.Log("Too far away, need to find a tool");
            }
            
        }
    }

    bool CheckPosition () {
        Vector3 p = currentItem.transform.position;
        Vector3 q = transform.position;
        return (p - q).magnitude < minDistance;
    }

    // Show the pickup guide (text and background)
    private void ShowPickupGuide()
    {
        pickUpGuide.SetActive(true);
        startPick?.Invoke();
    }

    // Hide the pickup guide (text and background)
    public void HidePickupGuide()
    {
        UpdateCurrentItem(null);
        pickUpGuide.SetActive(false);
        endPick?.Invoke();
    }
}
