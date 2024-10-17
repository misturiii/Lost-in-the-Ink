using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorChange : MonoBehaviour
{
    [SerializeField] float duration = 5f;
    protected Material mat;
    public float progress = 0;
    InputAction inputAction;

    protected static readonly int progressId = Shader.PropertyToID("_Progress");

    public void Awake () {
        mat = GetComponent<Renderer>().material;
        inputAction = FindObjectOfType<InputActionManager>().inputActions.UI.Trigger;
        inputAction.performed += Change;
    }

    protected IEnumerator AutoColorChange () {
        inputAction.performed -= Change;
        while (progress < 1) {
            progress += Time.deltaTime / duration;
            mat.SetFloat(progressId, progress);
            yield return null;
        }
    }

    void Change (InputAction.CallbackContext context) {
        StartCoroutine(AutoColorChange());
    }
}
