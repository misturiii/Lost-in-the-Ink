using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToolSticker : Sticker
{
    ToolBehaviour behaviour;
    GraphicRaycaster raycaster;

    public override void Initialize(Item item)
    {
        base.Initialize(item);
        behaviour = GetComponent<ToolBehaviour>();
        raycaster = stickerPanel.GetComponent<GraphicRaycaster>();
        lineColor = FunctionLibrary.LineColor1;
    }
    public override void Drop(InputAction.CallbackContext context)
    {
        sketchbookGuide.DisplayDragGuide();
        inventoryBox.Select();
        PointerEventData data = new PointerEventData(EventSystem.current) { position = transform.position};
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        Debug.Log("Drop position" + data.position);

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
