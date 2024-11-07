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
    [SerializeField] public Transform copy = null;
    public float rototation = 0;
    public int index;
    public AudioClip dropStickerAudioClip;
    public AudioClip duplicateStickerClip; 
    public AudioClip rotateStickerClip; 
    float overlapCheck = 100;
    
    public override void Initialize(Item item) {
        base.Initialize(item);
        enabled = true;
        lineColor = FunctionLibrary.LineColor2;
    }

    private void SetNavigationMode (Navigation.Mode mode) {
        Navigation nav = navigation;
        nav.mode = mode;
        navigation = nav;
    }

    public override void Drop(InputAction.CallbackContext context) {
        sketchbookGuide.DisplayDragGuide();
        // in inventory and drag failed
        if (inventoryBox && transform.localPosition.x >= -100) {
            inventoryBox.UpdateCount(++item.count);
            inventoryBox.Select();
            if (item.count == 1) {
                transform.localPosition = InitialPositon;    
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
                if (sticker && sticker != this && (transform.position - sticker.transform.position).magnitude < sticker.overlapCheck) {
                    sketchbookGuide.DisplayResult(FunctionLibrary.HighlightString("Cannot overlap"));
                    transform.localPosition = InitialPositon;
                    if (inventoryBox) {
                        inventoryBox.UpdateCount(++item.count);
                        inventoryBox.Select();
                        if (item.count == 1) {
                            transform.localPosition = InitialPositon;    
                        } else {
                            Delete();
                        }
                    }
                    return;
                }
            }
            GenerateObject(false);
            Select();
             if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(dropStickerAudioClip);
            }
        }
    }

    public override void OnSelect(BaseEventData data)
    {
        base.OnSelect(data);
        if (inventoryDisplay) {
            inventoryDisplay.SelectSketchbook(this);
        }
    }

    public override void Delete()
    {   
        base.Delete();
        if (copy) {
            Destroy(copy.gameObject);
            inventoryDisplay.RemoveFromSketchbook(this);  
        }
        Destroy(gameObject); 

    }

    public bool TrashCan() {
        if (item.total <= 1) {
            return false;
        } else {
            item.total -= 1;
           
            Delete();
            return true;
        }
    }

    public bool Camera() {
        if (item.total == 9) {
            return false;
        } else {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(duplicateStickerClip);
            }
            inventoryDisplay.InventoryAdd(item);
            return true;
        }
    }

    public bool Mirror(){
         if (item != null)
        {
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(rotateStickerClip);
            }
            transform.Rotate(0, 0, 45);
            rototation -= 45;
            GenerateObject(false);
            return true;
        }else{
            return false;
        }
    }

    public void GenerateObject(bool isComplete){
        if(!copy){
            if (inventoryBox) {
                inventoryDisplay = inventoryBox.inventoryDisplay;
                inventoryBox.RemoveSticker();
                inventoryBox = null;
            }
            transform.SetParent(stickerPanel);
            copy = Instantiate(prefab, GameObject.FindWithTag("World").transform).transform;
            SetNavigationMode(Navigation.Mode.Automatic);
            material.SetFloat(lineWidthId, lineWidth / scale);
            transform.localScale *= scale;
            ColorChange cc = copy.AddComponent<ColorChange>(); 
            cc.item = item;
            cc.isComplete = isComplete;
        }
        if (canvas) {
            Destroy(GetComponent<GraphicRaycaster>());
            Destroy(GetComponent<Canvas>());
            canvas = null;
        }
        Vector3 worldPosition = FunctionLibrary.BookToWorld(transform.localPosition);
        worldPosition.y = prefab.transform.localPosition.y;
        copy.localPosition = worldPosition;
        copy.localEulerAngles = new Vector3(0, rototation, 0);
        copy.GetComponent<ColorChange>().Reset();
    }

    public void ResetObject() {
        copy.GetComponent<ColorChange>().Reset();
    }
}
