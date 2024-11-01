using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorChange : MonoBehaviour
{
    float duration = 0.12f;
    float maxAngle = 20f;
    Material[] mats;
    InputActions inputActions;
    Camera cam;
    bool isVisible;
    Rigidbody rb;
    MeshCollider meshCollider;
    bool IsSleeping = false, IsCorrect = false;
    public string itemName;
    ItemObject[] objects;

    protected static readonly int 
        durationId = Shader.PropertyToID("_Duration"), 
        startTimeId = Shader.PropertyToID("_StartTime");

    void Start () {
        meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        rb = gameObject.AddComponent<Rigidbody>();
        isVisible = false;
        mats = GetComponent<Renderer>().materials;
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Trigger.performed += CloseSketchBook;
        inputActions.Player.Trigger.performed += OpenSketchBook;
        cam = Camera.main;
        StartCoroutine(SleepCheck());
        objects = GetComponentsInChildren<ItemObject>(true);
    }

    void Update () {
        if (IsSleeping) {
            rb.isKinematic = true;
            meshCollider.convex = false;
        } else {
            meshCollider.convex = true;
            rb.isKinematic = false;
        }
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
        IsSleeping = false;
        if (rb) { rb.velocity = Vector3.zero; }
    }

    void CloseSketchBook (InputAction.CallbackContext context) {
        isVisible = true;
    }

    void OpenSketchBook (InputAction.CallbackContext context) {
        isVisible = false;
    }

    void OnTriggerEnter (Collider collider) {
        if (collider.name == itemName) {
            IsCorrect = true;
            foreach (var obj in objects) {
                obj?.gameObject.SetActive(true);
            }
            GetComponentInParent<LayoutCheck>().Check();
            Destroy(collider.gameObject);
        } 
    }
}
