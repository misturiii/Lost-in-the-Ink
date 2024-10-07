using UnityEngine;

public class ItemSticker : Sticker
{
    protected Transform copy = null;
    [SerializeField] protected GameObject prefab;

    protected override void Initialize() {}
    protected override void UseStickerBefore() {}
    protected override void UseStickerAfter() {
        if (!copy) {
            copy = Instantiate(prefab).transform;
        }
        copy.localPosition = TransformationFunction.BookToWorld(transform.localPosition);
        copy.localEulerAngles = Vector3.zero;
    }
}
