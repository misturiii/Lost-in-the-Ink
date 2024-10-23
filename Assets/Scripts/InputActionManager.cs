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

    void Quit (InputAction.CallbackContext context) {
        Application.Quit();
    }
}
