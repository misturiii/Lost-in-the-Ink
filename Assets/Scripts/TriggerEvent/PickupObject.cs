using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI; // For Image
using TMPro; // For TextMeshPro

public class PickupObject : MonoBehaviour
{
    Inventory inventory;                // Reference to the Inventory ScriptableObject
    public InventoryDisplay inventoryDisplay;  // Reference to the InventoryDisplay
    private GameObject currentItem;      // Track the currently detected item
    InputActions inputActions;

    public GameObject pickUpGuide;
    float rayDistance = 6f;  // Distance the ray can detect
    public float minDistance = 3f;
    void Start()
    {
        inventory = Resources.Load<Inventory>("PlayerInventory");
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.GrabSticker.Enable();
        inputActions.Player.GrabSticker.performed += Grab;

        // Initially hide the pickup text and background
        pickUpGuide.SetActive(false);
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
                currentItem = hit.collider.gameObject; // Store the currently detected item
                currentItem.GetComponent<ItemObject>().Enter();      
                ShowPickupGuide();          
            }
            else{
                
                HidePickupGuide();// Do not show guide
            }
        }
        else{
            if(currentItem){
                currentItem.GetComponent<ItemObject>().Exit();
                currentItem = null;
            }
            HidePickupGuide();
        }

    }

    void Grab(InputAction.CallbackContext context)
    {   
        // Check for the E key press and if there's a current item
        if (context.performed && currentItem != null)
        {
            if ((currentItem.transform.position - transform.localPosition).magnitude < minDistance) {
                ItemObject itemObject = currentItem.GetComponent<ItemObject>();
                if (itemObject != null)
                {
                    inventory.Add(itemObject.item); // Add the item to the inventory
                    Debug.Log("Picked up item: " + itemObject.item.itemName);

                    // Destroy the item from the scene
                    Destroy(currentItem);
                    currentItem = null; // Reset current item

                    // Hide the pickup text and background after pickup
                    HidePickupGuide();
                }
                else
                {
                    Debug.LogError("ItemObject component is missing on: " + currentItem.name);
                }
            } else {
                Debug.Log("Too far away, nned to find a tool");
            }
            
        }
    }

    // Method to manually check if player is in the pickup area
    public void CheckForPickupItem()
    {
        if (currentItem != null && currentItem.CompareTag("PickableItem"))
        {
            // Manually display pickup guide if player is in the area after dialogue ends
            Debug.Log("Player is in the pickup area after dialogue ended, item is now pickable.");
            ShowPickupGuide();
        }
    }

    // Show the pickup guide (text and background)
    private void ShowPickupGuide()
    {
        pickUpGuide.SetActive(true);
    }

    // Hide the pickup guide (text and background)
    private void HidePickupGuide()
    {
        pickUpGuide.SetActive(false);
    }
}
