using System;
using UnityEngine;

public class ToolSticker : Sticker
{
    [SerializeField] float distance = 10;
    [SerializeField] public String itemName;
    ToolBehaviour behaviour;
    Transform target;

    protected override void Initialize() {
        enabled = true;
        behaviour = GetComponent<ToolBehaviour>();
        target = behaviour.GetTarget();
    }

    protected override void UseStickerAfter() {}

    protected override void UseStickerBefore()
    {
        if ((transform.position - target.position).magnitude > distance) {
            if (InInventory()) {
                transform.localPosition = Vector3.zero;
            } else {
                
            }
        } else {
            behaviour.StartBehaviour();
        }
    }
}
