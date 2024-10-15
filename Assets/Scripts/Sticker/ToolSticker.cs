using UnityEngine;

public class ToolSticker : Sticker
{
    Transform target;
    Vector2 tolerance  = new Vector2(100, 140);
    [SerializeField] protected GameObject prefab;
    bool removed = false;

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
        inventoryDisplay.RemoveSticker(this);
        Destroy(gameObject);
    }
}
