using UnityEngine;
using UnityEngine.InputSystem;

public class ToolSticker : Sticker
{
    Transform target;
    Vector2 tolerance;
    [SerializeField] protected GameObject prefab;
    [SerializeField] string targetName;
    [SerializeField] Vector3 prefabPosition;
    Transform item;
    protected override void Initialize() {
        enabled = true;
        target = GameObject.Find(targetName).transform;
        tolerance = 0.5f * target.GetComponent<RectTransform>().sizeDelta;
        Debug.Log($"sticker {name} has tolerance {tolerance}");
    }


    public override void Drop(InputAction.CallbackContext context) {
        for (int i = 0; i < 2; i++) {
            if (Mathf.Abs(transform.position[i] - target.position[i]) > tolerance[i]) {
                transform.localPosition = Vector3.zero;
                sketchbookGuide.DisplayResult(false);
                return;
            }
        }
        sketchbookGuide.DisplayResult(true);
        item = Instantiate(prefab).transform;
        item.position = prefabPosition;
        target.GetComponent<StickerChange>().Change();
        inventoryBox.RemoveSticker();
        inputActions.UI.Click.canceled -= Drop;
        Destroy(gameObject);
    }
}
