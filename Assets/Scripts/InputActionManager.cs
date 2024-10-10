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
        inputActions.Global.Quit.performed += Quit;
        inputActions.Player.Trigger.performed += OpenInventory;
        inputActions.UI.Trigger.performed += CloseInventory;
        inputActions.UI.Disable();
        inputActions.Puzzle.Disable();
    }

    void Quit (InputAction.CallbackContext context) {
        Application.Quit();
    }

    void OpenInventory (InputAction.CallbackContext context) {
        inputActions.Player.Disable();
        inputActions.UI.Enable();
    }

    void CloseInventory (InputAction.CallbackContext context) {
        inputActions.Player.Enable();
        inputActions.UI.Disable();
    }
}
