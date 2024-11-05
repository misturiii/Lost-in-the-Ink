using System.Collections;
using TreeEditor;
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
    MeshCollider meshCollider;
    bool IsSleeping = false, IsCorrect = false;
    public string itemName;
    ItemObject[] objects;
    public Item item;

    protected static readonly int 
        durationId = Shader.PropertyToID("_Duration"), 
        startTimeId = Shader.PropertyToID("_StartTime");

    void Start () {
        meshCollider = gameObject.AddComponent<MeshCollider>();
        mats = GetComponent<Renderer>().materials;
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Trigger.performed += CloseSketchBook;
        inputActions.Player.Trigger.performed += OpenSketchBook;
        cam = Camera.main;
        StartCoroutine(SleepCheck());
        objects = GetComponentsInChildren<ItemObject>(true);
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

    IEnumerator SleepCheck () {
        while (true) {
            Vector3 p = transform.position;
            yield return new WaitForSeconds(2f);
            IsSleeping = (p - transform.position).magnitude < 0.1f;
        }
    }

    public void Reset () {
        if (item.Check(transform.position)) {
            Trigger();
        }
        foreach (var obj in objects) {
            if ((item.total - item.count) == obj.appearCount && !obj.item.IsPicked) {
                obj.enabled = true;
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
        foreach (var obj in objects) {
            obj?.gameObject.SetActive(true);
        }
        GetComponentInParent<LayoutCheck>().Check(); 
    }

    bool AngleCheck (Transform other) {
        float a = other.transform.eulerAngles.y;
        float b = transform.eulerAngles.y;
        return Mathf.Abs(a - b) % 360 < maxAngle;
    }

}
