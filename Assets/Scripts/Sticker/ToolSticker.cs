using System;
using UnityEngine;

public class ToolSticker : Sticker
{
    [SerializeField] float distance = 10;
    [SerializeField] String itemName;
    ToolBehaviour behaviour;
    Transform target;

    protected override void Initialize() {
        enabled = true;
        behaviour = GetComponent<ToolBehaviour>();
        target = GameObject.FindWithTag("TicketTarget").transform;
    }

    protected override void UseStickerAfter()
    {
        behaviour.StartBehaviour();
    }

    protected override void UseStickerBefore()
    {
        if ((transform.position - target.position).magnitude > distance) {
            if (InInventory()) {
                transform.localPosition = Vector3.zero;
            } else {
                inventory.Add(Resources.Load<Item>(itemName));
                ResetSelect(inventory.items.Count);
                behaviour.EndBehaviour();
                Destroy(this);
            }
            
        }
    }

    public void ResetSelect(int i) {
        inventoryDisplay.ResetSelect(i);
    }
}
