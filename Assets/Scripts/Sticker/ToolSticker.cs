using System;
using UnityEngine;

public class ToolSticker : Sticker
{
    [SerializeField] float distance = 10;
    Transform target;
    Vector2 tolerance  = new Vector2(125, 160);
    [SerializeField] protected GameObject prefab;

    protected override void Initialize() {
        enabled = true;
        target = GameObject.FindWithTag("Puzzle").transform;
    }

    protected override void UseStickerAfter() {}

    protected override void UseStickerBefore()
    {

        for (int i = 0; i < 2; i++) {
            if (Mathf.Abs(transform.position[i] - target.position[i]) > tolerance[i]) {
                transform.localPosition = Vector3.zero;
                return;
            }
        }
        Instantiate(prefab).transform.position = TransformationFunction.BookToWorld(target.localPosition);
        Destroy(gameObject);
    }
}
