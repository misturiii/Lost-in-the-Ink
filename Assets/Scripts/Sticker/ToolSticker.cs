using UnityEngine;

public class ToolSticker : Sticker
{
    Transform target;
    Vector2 tolerance  = new Vector2(100, 140);
    [SerializeField] protected GameObject prefab;
    [SerializeField] string targetName;
    [SerializeField] Vector3 prefabPosition;

    protected override void Initialize() {
        enabled = true;
        target = GameObject.Find(targetName).transform;
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
        Transform item = Instantiate(prefab).transform;
        item.position = prefabPosition;
        item.GetComponent<ColorChange>().Initialize();
        // inventoryDisplay.RemoveSticker(this);
        target.GetComponent<StickerChange>().Change();
        Destroy(gameObject);
    }
}
