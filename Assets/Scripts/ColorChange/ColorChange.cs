using UnityEngine;
using UnityEngine.InputSystem;

public class ColorChange : MonoBehaviour
{
    float duration = 0.12f;
    Material[] mats;
    InputActions inputActions;
    Camera cam;
    bool isVisible;
    Rigidbody rb;
    MeshCollider MeshCollider;
    bool IsSleeping = false;

    protected static readonly int 
        durationId = Shader.PropertyToID("_Duration"), 
        startTimeId = Shader.PropertyToID("_StartTime");

    void Start () {
        isVisible = false;
        mats = GetComponent<Renderer>().materials;
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Trigger.performed += CloseSketchBook;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = gameObject.AddComponent<Rigidbody>();
        MeshCollider = GetComponent<MeshCollider>();
        MeshCollider.convex = true;
    }

    void Update () {
        if (rb.IsSleeping() != IsSleeping) {
            rb.isKinematic = !IsSleeping;
            MeshCollider.convex = IsSleeping;
            IsSleeping = !IsSleeping;
        }
        if (isVisible && mats[0].GetFloat(durationId) == 0) {
            foreach (Material mat in mats) {
                Vector2 viewPos = cam.WorldToViewportPoint(transform.position);
                if (0 < viewPos.x && viewPos.x < 1 && 0 < viewPos.y && viewPos.y < 1) {
                    mat.SetFloat(durationId, duration);
                    mat.SetFloat(startTimeId, Time.time);
                }
            }
        }
    }

    void CloseSketchBook (InputAction.CallbackContext context) {
        isVisible = true;
    }
}
