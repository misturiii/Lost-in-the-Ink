using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToolSticker : Sticker
{
    ToolBehaviour behaviour;

    public override void Initialize(Item item)
    {
        base.Initialize(item);
        behaviour = GetComponent<ToolBehaviour>();
        lineColor = FunctionLibrary.LineColor1;
    }
    public override void Drop(InputAction.CallbackContext context)
    {
        sketchbookGuide.DisplayDragGuide();
        inventoryBox.Select();
        
        List<RaycastResult> results = DetectOverlap();

        foreach (RaycastResult result in results)
        {
            Debug.Log("Hit UI element: " + result.gameObject.name);
            ItemSticker sticker = result.gameObject.GetComponent<ItemSticker>();
            if (sticker) {
                sketchbookGuide.DisplayResult(behaviour.StartBehaviour(sticker));
                break;
            }
        }
        transform.localPosition = Vector3.zero;
    }

    public override void Delete()
    {
        base.Delete();
        Destroy(gameObject);
    }
}
