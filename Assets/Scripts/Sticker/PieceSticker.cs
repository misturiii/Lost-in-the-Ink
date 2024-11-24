using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class PieceSticker : Sticker
{
    [SerializeField] private GameObject newStickerPrefab; // New sticker to create
    [SerializeField] public Item newStickerItem; // New sticker to create
    private InventoryDisplay inventoryDisplay1 = null;
   
    public ItemSticker newItemSticker;
    

    public override void Initialize(Item item)
    {
        base.Initialize(item);
        SetUp();
    }

    protected override void SetLineColor()
    {
        lineColor = FunctionLibrary.LineColor1;
        lineColor2 = new Color(0.75f, 0.75f, 0.75f);
    }

    public override void Drop(InputAction.CallbackContext context)
    {
        base.Drop(context);
        sketchbookGuide.DisplayDragGuide();
        inventoryBox.Select();
        
        List<RaycastResult> results = DetectOverlap();

        foreach (RaycastResult result in results)
        {   
            Debug.Log("result is " + result);
            ItemSticker targetSticker = result.gameObject.GetComponent<ItemSticker>();
            if (targetSticker && IsMatch(targetSticker))
            {
                HandleMatch(targetSticker);
            }  
        }
        // Reset position if no match is found
        inventoryBox.UpdateCount(item.count);
        transform.localPosition = Vector3.zero - pivotOffset;
    }

    private bool IsMatch(ItemSticker targetSticker)
    {   
        Debug.Log($"{targetSticker.item.itemName} and {item.itemName}");
        Debug.Log("copy is " + targetSticker.copy);

        // Check if the target sticker's item is compatible with this item's compatible items
        return item.compatibleItems != null && (item.compatibleItems == targetSticker.item.itemName);
    }


    private void HandleMatch(ItemSticker targetSticker)
    {
        // First, destroy the copy of the target sticker if it exists
        if (targetSticker.copy != null) // Use the copy from the targetSticker
        {
            Destroy(targetSticker.copy.gameObject);
            targetSticker.copy = null; // Optionally nullify the reference
        }
        item.count--;
        targetSticker.item.RemovePieceCheck();
        
        // Create a new sticker at the original position of the target sticker
        GameObject newSticker = Instantiate(newStickerPrefab, targetSticker.transform.position, Quaternion.identity, stickerPanel);
        newSticker.transform.localPosition = targetSticker.transform.localPosition;
        // Initialize or update the new sticker
        newItemSticker = newSticker.GetComponent<ItemSticker>();
        if (newItemSticker != null)
        {   
            Debug.Log("the current targetSticker item is "+ newStickerItem);
            newItemSticker.numRotate = targetSticker.numRotate;
            // Call the Delete method on the target sticker to handle its destruction
            targetSticker.Delete();
          
            inventoryDisplay1 = GameObject.FindWithTag("Sketchbook").transform.GetChild(3).GetComponent<InventoryDisplay>();

            if (inventoryDisplay1 == null) {
                Debug.Log("InventoryDisplay component not found on the specified child of Sketchbook.");
            }
            newItemSticker.inventoryDisplay = inventoryDisplay1;
            // newStickerItem.count++;
            newStickerItem.total++;
            newItemSticker.Initialize(newStickerItem);
            newItemSticker.GenerateObject(true); // Generate the associated 3D object
            newItemSticker.Drop(new InputAction.CallbackContext());
        }
        inventoryBox.RemoveSticker();
        Delete(); // This could be adjusted based on your game logic

        Debug.Log("Match found! Replaced with a new sticker and 3D object.");
    }

    public override void Delete()
    {   
        base.Delete(); // Ensure base cleanup happen
        Destroy(gameObject); // This will destroy the current sticker
    }
}