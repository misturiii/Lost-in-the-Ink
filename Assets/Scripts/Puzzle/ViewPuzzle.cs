using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class ViewPuzzle : MonoBehaviour
{
    [SerializeField] float duration = 2;
    [SerializeField] Volume volume;
    Transform sticker;
    Transform puzzle;
    Camera view;
    Player player;
    InputActions inputActions;
    float offset;
    bool detected;

    void Start()
    {
        view = GameObject.FindGameObjectWithTag("ViewCamera").GetComponent<Camera>();
        player = GetComponent<Player>();
        detected = false;

        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.ViewPuzzle.Enable();
        inputActions.Player.ViewPuzzle.performed += View;
    }

    void View (InputAction.CallbackContext context) {
        if (detected) {
            if (view.enabled) {
                view.enabled = false;
                player.enabled = true;
                volume.enabled = false;
                SetLayer(puzzle, 0);
                
            } else {
                SetViewCamera();
                view.enabled = true;
                player.enabled = false;
                volume.enabled = true;
                SetLayer(puzzle, 7);
            }
        }
    }

    void SetViewCamera () {
        view.transform.localEulerAngles = puzzle.eulerAngles;
        view.transform.localPosition = puzzle.position - puzzle.forward * offset;
    }

    void SetLayer (Transform transform, int layer) {
        for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.layer = layer;
        }
    }

    public void StartZoomIn () {
        StartCoroutine(ZoomIn());
    }

    IEnumerator ZoomIn () {
        Vector3 initialPosition = sticker.localPosition;
        float i = 0; 
        while (i < 1) {
            i += Time.deltaTime / duration;
            sticker.localPosition = Vector3.Lerp(initialPosition, new Vector3(0, 0, -0.1f), Mathf.Min(i, 1));
            yield return null;
        }
        sticker.GetComponent<Collider>().enabled = true;
    }

    void OnTriggerEnter (Collider collider) {
        if (collider.tag == "Puzzle") {
            puzzle = collider.transform;
            sticker = puzzle.GetChild(0);
            if (sticker.tag == "PickableItem") {
                sticker.GetComponent<Collider>().enabled = false;
            }
            detected = true;
            offset = sticker.parent.GetComponent<Puzzle>().Offset();
        }
    } 

    void OnTriggerExit (Collider collider) {
        if (collider.tag == "Puzzle") {
            puzzle = null;
            sticker = null;
            detected = false;
        }
    }
}
