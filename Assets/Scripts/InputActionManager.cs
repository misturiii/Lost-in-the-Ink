using UnityEngine;
public class InputActionManager : MonoBehaviour
{
    public InputActions inputActions;
    [SerializeField] Texture2D hand1, hand2;
    Vector2 hotspot = new Vector2(32, 32);
    public bool isMouse;
  
    void Awake()
    {
        inputActions = new InputActions();
        inputActions.Global.Enable();
        inputActions.Player.Enable();
        // inputActions.Global.Quit.performed += Quit;
        inputActions.UI.Disable();
        SetCursorMode(false);
        isMouse = PlayerPrefs.GetString("InputDevice", "keyboard") == "keyboard";
    }

    public void SetPlayerActive(bool active) {
        if (active) {
            inputActions.Player.Enable();
            inputActions.UI.Disable();
        } else {
            inputActions.UI.Enable();
            inputActions.Player.Disable();
        }
        SetCursorMode(!active);
    }

    public void SetAllActive (bool active) {
        if (active) {
            inputActions.Player.Enable();
            inputActions.UI.Enable();
        } else {
            inputActions.UI.Disable();
            inputActions.Player.Disable();
        }
        SetCursorMode(!active);
    }

    void OnDestroy () {
        inputActions.Disable();
    }

    void SetCursorMode(bool active) {
        if (!active || !isMouse) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void SetDragCursor (bool isDragging) {
        Cursor.SetCursor(isDragging ? hand2 : hand1, hotspot, CursorMode.Auto);
    }
}
