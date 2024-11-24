using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemSticker : Sticker
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] float scale = 1;
    [SerializeField] public InventoryDisplay inventoryDisplay = null;
    public Transform copy = null;
    public int numRotate = 0;
    float overlapCheck = 50;
    [SerializeField] Sprite[] rotatedImages;
    [SerializeField] bool notStraight;

    private Vector3 lastPosition; 

    private GameObject moveEffectPrefab; 
    
    public override void Initialize(Item item) {
        base.Initialize(item);
        enabled = true;
        moveEffectPrefab = Resources.Load<GameObject>("MoveEffect");
        if (copy) {
            copy.GetComponent<ColorChange>().Reset();
        }
        image.sprite = rotatedImages[numRotate];
    }

    protected override void SetLineColor()
    {
        lineColor = FunctionLibrary.LineColor2;
        lineColor2 = Color.white;
    }

    public override void Drop(InputAction.CallbackContext context) {
        base.Drop(context);
        sketchbookGuide.DisplayDragGuide();
        // in inventory and drag failed
        if (inventoryBox && inventoryBox.transform.position.x - transform.position.x < 100) {
            inventoryBox.UpdateCount(item.count);
            inventoryBox.Select();
            if (item.count == 1) {
                transform.localPosition = -pivotOffset;    
            } else {
                Delete();
            }
        } else {
            // in sketchbook
            List<RaycastResult> results = DetectOverlap();
            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit UI element: " + result.gameObject.name);
                ItemSticker sticker = result.gameObject.GetComponent<ItemSticker>();
                if (sticker && sticker != this && Vector3.Distance(transform.position, sticker.transform.position) < sticker.overlapCheck + overlapCheck) {
                    sketchbookGuide.DisplayResult(FunctionLibrary.HighlightString("Cannot overlap"));
                    if (inventoryBox) {
                        inventoryBox.UpdateCount(item.count);
                        inventoryBox.Select();
                        if (item.count == 1) {
                            transform.localPosition = -pivotOffset;    
                        } else {
                            Delete();
                        }
                    } else {
                        transform.localPosition = InitialPositon;
                    }
                    return;
                }
            }
            GenerateObject(false);
            Select();
            audioManager.Play(StickerAudio.drop);
            Debug.Log("Finish Drop");
        }
    }

    public override void OnSelect(BaseEventData data)
    {
        base.OnSelect(data);
        if (inventoryDisplay) {
            inventoryDisplay.Select(this);
        }
    }

    public override void Delete()
    {   
        base.Delete();
        if (copy) {
            Destroy(copy.gameObject);
        }
        inventoryDisplay?.RemoveFromSketchbook(this);  
        Debug.Log("Sticker destroyed");
        Destroy(gameObject); 
    }

    public bool TrashCan(float input) {
        if (item.total <= 1) {
            sketchbookGuide.DisplayResult(FunctionLibrary.HighlightString($"{item.itemName} sticker is unique, cannot delete it"));
            return false;
        } else {
            item.total -= 1;
            audioManager.Play(StickerAudio.trash);
            Delete();
            sketchbookGuide.DisplayResult($"Successfully deleted {item.itemName} sticker");
            return true;
        }
    }

    public bool Camera(float input) {
        if (item.total == 9) {
            sketchbookGuide.DisplayResult(FunctionLibrary.HighlightString($"Reach the maximum, cannot duplicate anymore"));
            return false;
        } else {
            audioManager.Play(StickerAudio.cam);
            inventoryDisplay.AddToInventory(item);
            sketchbookGuide.DisplayResult($"Successfully duplicated {item.itemName} sticker");
            return true;
        }
    }

    public bool Mirror(float sign){
         if (item != null)
        {
            audioManager.Play(StickerAudio.rubik);
            numRotate = (numRotate + (int)sign + rotatedImages.Length) % rotatedImages.Length;
            image.sprite = rotatedImages[numRotate];
            GenerateObject(false);
            sketchbookGuide.DisplayResult($"Successfully rotated {item.itemName} sticker");
            return true;
        }else{
            return false;
        }
    }

    public void SetOutline () {
        lineWidth /= scale;
        material.SetFloat(lineWidthId, lineWidth);
    }

    public void GenerateObject(bool isComplete){
        if(!copy){
            if (inventoryBox) {
                inventoryDisplay = inventoryBox.inventoryDisplay;
                inventoryBox.RemoveSticker();
                inventoryBox = null;
                item.count--;
            }
            transform.SetParent(stickerPanel);
            copy = Instantiate(prefab, GameObject.FindWithTag("World").transform).transform;
            SetOutline();
            transform.localScale *= scale;
            ColorChange cc = copy.AddComponent<ColorChange>(); 
            cc.item = item;
            cc.isComplete = isComplete;
            pivotOffset *= scale;
            if (canvas) {
                Destroy(GetComponent<GraphicRaycaster>());
                Destroy(canvas);
                canvas = null;
            }
        }
        Vector3 worldPosition = FunctionLibrary.BookToWorld(transform.localPosition);
        worldPosition.y = prefab.transform.localPosition.y;

        if (moveEffectPrefab != null && worldPosition != lastPosition)
        {
            GameObject moveEffect = Instantiate(moveEffectPrefab, lastPosition, Quaternion.identity);
            moveEffect.transform.LookAt(worldPosition); 
            moveEffect.GetComponent<ParticleSystem>().Play(); 
            Destroy(moveEffect, moveEffect.GetComponent<ParticleSystem>().main.duration); 
        }
        lastPosition = worldPosition; 

        copy.localPosition = worldPosition;
        copy.localEulerAngles = new Vector3(0, numRotate * 90 - (notStraight ? 45 : 0), 0);
        copy.GetComponent<ColorChange>().Reset();
        Debug.Log("Update copy");
    }
}
