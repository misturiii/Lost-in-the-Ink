using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorChange : MonoBehaviour
{
    float duration = 0.12f;
    float maxAngle = 10f;
    Material[] mats;
    InputActions inputActions;
    Camera cam;
    bool isVisible = true;
    bool IsCorrect = false;
    public string itemName;
    ItemObject[] objects;
    public Item item;

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
        if (IsCorrect && isVisible && mats[0].GetFloat(durationId) == 0) {
            foreach (Material mat in mats) {
                Vector2 viewPos = cam.WorldToViewportPoint(transform.position);
                if (0 < viewPos.x && viewPos.x < 1 && 0 < viewPos.y && viewPos.y < 1) {
                    mat.SetFloat(durationId, duration);
                    mat.SetFloat(startTimeId, Time.time);
                }
            }
        }
    }

    public void Reset () {
        if (item.Check(transform.position)) {
            Trigger();
        }
        if (objects != null) {
            foreach (var obj in objects) {
                if ((item.total - item.count) >= obj.appearCount && !obj.item.stickerPlaced) {
                    obj.gameObject.SetActive(true);
                    obj.item.stickerPlaced = true;
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
        if (objects != null) {
            foreach (var obj in objects) {
                obj?.gameObject.SetActive(true);
            }
        }
        GetComponentInParent<LayoutCheck>().Check(); 
    }

    bool AngleCheck (Transform other) {
        float a = other.transform.eulerAngles.y;
        float b = transform.eulerAngles.y;
        return Mathf.Abs(a - b) % 360 < maxAngle;
    }

    void OnDestroy() {
        if (objects != null) {
            foreach (var obj in objects) {
                if (obj) {
                    obj.item.stickerPlaced = false;
                }
            }
        }
    }

}
