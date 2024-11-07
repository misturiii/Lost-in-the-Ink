using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorChange : MonoBehaviour
{
    float duration = 0.12f;
    Material[] mats;
    InputActions inputActions;
    Camera cam;
    bool isVisible = false;
    bool IsCorrect = false;
    public string itemName;
    ItemObject[] objects;
    public Item item;
    bool changed = false;
    public bool isComplete;

    protected static readonly int 
        durationId = Shader.PropertyToID("_Duration"), 
        startTimeId = Shader.PropertyToID("_StartTime");

    void Awake () {
        objects = GetComponentsInChildren<ItemObject>(true);
        gameObject.AddComponent<MeshCollider>();
        mats = GetComponent<Renderer>().materials;
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Trigger.performed += CloseSketchBook;
        inputActions.Player.Trigger.performed += OpenSketchBook;
        cam = Camera.main;
    }

    void Update () {
        Vector2 viewPos = cam.WorldToViewportPoint(transform.position);
        bool temp = isVisible && 0 < viewPos.x && viewPos.x < 1 && 0 < viewPos.y && viewPos.y < 1;
        if (!changed && temp && (isComplete || IsCorrect)) {
            changed = true;
            foreach (Material mat in mats) {
                mat.SetFloat(durationId, duration);
                mat.SetFloat(startTimeId, Time.time);
            }
        }
    }

    public void Reset () {
        if (!changed) {
            if (isComplete) {
                item.Check();
            } else if (item.Check(transform.position, transform.eulerAngles)) {
                Trigger();
            }
        }
        if (objects != null) {
            foreach (var obj in objects) {
                if ((item.total - item.count) >= obj.appearCount && !obj.item.stickerPlaced) {
                    obj.item.stickerPlaced = true;
                    obj.gameObject.SetActive(true);
                }
            }
        }
    }

    void CloseSketchBook (InputAction.CallbackContext context) {
        isVisible = true;
    }

    void OpenSketchBook (InputAction.CallbackContext context) {
        isVisible = false;
    }
    void Trigger () {
        IsCorrect = true;
        GetComponentInParent<LayoutCheck>().Check(); 
    }

    void OnDestroy() {
        if (objects != null) {
            foreach (var obj in objects) {
                if (obj && obj.gameObject.activeSelf) {
                    obj.item.stickerPlaced = false;
                }
            }
        }
    }

}
