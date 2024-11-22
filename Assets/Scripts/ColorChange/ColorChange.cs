using System.Collections.Generic;
using System.Linq;
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
    int checkedIndex = -1;
    GameObject particle;

    protected static readonly int 
        durationId = Shader.PropertyToID("_Duration"), 
        startTimeId = Shader.PropertyToID("_StartTime");

    void Initialize () {
        mats = new Material[0];
        objects = GetComponentsInChildren<ItemObject>(true);
        TraverseChildComponents(transform);
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Trigger.performed += CloseSketchBook;
        inputActions.Player.Trigger.performed += OpenSketchBook;
        cam = Camera.main;
        particle = GetComponentInChildren<ParticleSystem>(true)?.gameObject;
    }

    void TraverseChildComponents (Transform t) {
        GetFromCurrent(t);
        Debug.Log($"childe {t.name} traversed");
        for (int i = 0; i < t.childCount; i++) {
            TraverseChildComponents(t.GetChild(i));
        }
    }

    void GetFromCurrent (Transform t) {
        MeshRenderer mr;
        t.TryGetComponent<MeshRenderer>(out mr);
        SkinnedMeshRenderer smr;
        t.TryGetComponent<SkinnedMeshRenderer>(out smr);
        if (mr) {
            mats = mats.Concat(mr.materials).ToArray();
            t.gameObject.AddComponent<MeshCollider>();
        } else if (smr) {
            mats = mats.Concat(smr.materials).ToArray();
            t.gameObject.AddComponent<MeshCollider>();
        }
    }

    void Update () {
        if (!cam) {
            Initialize();
        }
        Vector2 viewPos = cam.WorldToViewportPoint(transform.position);
        bool temp = isVisible && 0 < viewPos.x && viewPos.x < 1 && 0 < viewPos.y && viewPos.y < 1;
        if (!changed && temp && (isComplete || IsCorrect)) {
            ChangeColor(true);
        }
    }

    void ChangeColor (bool result) {
        changed = result;
        foreach (Material mat in mats) {
            mat.SetFloat(durationId, result ? duration : 0);
            mat.SetFloat(startTimeId, Time.time);
        }
        particle?.SetActive(result);
    }

    public void Reset () {
        int index = -1;
        item.Uncheck(checkedIndex);
        if (mats == null || mats.Length == 0) {
            Initialize();
        }
        if (isComplete) {
            index = item.Check();
        } else if ((index = item.Check(transform.position, transform.eulerAngles)) >= 0) {
            IsCorrect = true;
        } else {
            IsCorrect = false;
            ChangeColor(false);
        }
        checkedIndex = index;
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

    void OnDestroy() {
        if (objects != null) {
            foreach (var obj in objects) {
                if (obj && obj.gameObject.activeSelf) {
                    obj.item.stickerPlaced = false;
                }
            }
        }
        item.Uncheck(checkedIndex);
    }

}
