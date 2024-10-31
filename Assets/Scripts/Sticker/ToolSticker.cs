using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToolSticker : Sticker
{
    ToolBehaviour behaviour;
    GraphicRaycaster raycaster;

    public override void Initialize()
    {
        base.Initialize();
        behaviour = GetComponent<ToolBehaviour>();
        raycaster = GameObject.FindWithTag("Sketchbook").GetComponent<GraphicRaycaster>();
        material.SetColor(lineColorId, new Color(0.7f, 0.9f, 1f));
    }
    public override void Drop(InputAction.CallbackContext context)
    {
        PointerEventData data = new PointerEventData(EventSystem.current) { position = transform.position};
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        Debug.Log("Drop position" + data.position);

        foreach (RaycastResult result in results)
        {
            Debug.Log("Hit UI element: " + result.gameObject.name);
            ItemSticker sticker = result.gameObject.GetComponent<ItemSticker>();
            if (sticker) {
                behaviour.StartBehaviour(sticker);
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
