using UnityEngine;
using UnityEngine.InputSystem;

public class ColorChange : MonoBehaviour
{
    float duration = 0.12f;
    [SerializeField] Material mat;
    InputAction inputAction;
    Camera cam;
    bool isVisible;

    protected static readonly int 
        durationId = Shader.PropertyToID("_Duration"), 
        startTimeId = Shader.PropertyToID("_StartTime");

    void Awake () {
        isVisible = false;
        mat = GetComponent<Renderer>().material;
        inputAction = FindObjectOfType<InputActionManager>().inputActions.UI.Trigger;
        inputAction.performed += CloseSketchBook;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update () {
        if (isVisible && mat.GetFloat(durationId) == 0) {
            Vector2 viewPos = cam.WorldToViewportPoint(transform.localPosition);
            if (0 < viewPos.x && viewPos.x < 1 && 0 < viewPos.y && viewPos.y < 1) {
                mat.SetFloat(durationId, duration);
                mat.SetFloat(startTimeId, Time.time);
            }
        }
    }

    void CloseSketchBook (InputAction.CallbackContext context) {
        isVisible = true;
    }
}
