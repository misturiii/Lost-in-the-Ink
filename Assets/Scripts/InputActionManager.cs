using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionManager : MonoBehaviour
{
    public InputActions inputActions;
  
    void Awake()
    {
        inputActions = new InputActions();
        inputActions.Global.Enable();
        inputActions.Player.Enable();
        // inputActions.Global.Quit.performed += Quit;
        inputActions.UI.Disable();
    }

    public void SetPlayerActive(bool active) {
        if (active) {
            inputActions.Player.Enable();
            inputActions.UI.Disable();
        } else {
            inputActions.UI.Enable();
            inputActions.Player.Disable();
        }
    }

    public void SetAllActive (bool active) {
        if (active) {
            inputActions.Player.Enable();
            inputActions.UI.Enable();
        } else {
            inputActions.UI.Disable();
            inputActions.Player.Disable();
        }
    }

    void OnDestroy () {
        inputActions.Disable();
    }
}
