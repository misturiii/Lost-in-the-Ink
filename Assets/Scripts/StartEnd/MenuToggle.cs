using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MenuToggle : MonoBehaviour
{
    public GameObject menuUI;  // Reference to the UI panel for the menu
    private bool isPaused = false;
    InputActions inputActions;
    [SerializeField] GameObject current;

    void Start()
    {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Global.Quit.performed += Toggle;
    }

    void Toggle(InputAction.CallbackContext context)
    {
        // When Escape is pressed, toggle the menu
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        menuUI.SetActive(false);  // Hide the menu
        
        // inputActions.Player.Move.Enable();
        // inputActions.Player.Look.Enable(); 
        // inputActions.Player.Trigger.Enable();
        // inputActions.Player.Click.Enable();
        inputActions.UI.Enable();
        inputActions.Player.Enable();
        isPaused = false;
        SetCurrentSelection();
    }

    public void PauseGame()
{
    menuUI.SetActive(true);  // Show the menu (this ensures the menu is visible)
    // Time.timeScale = 0f;     // Pause the game
    isPaused = true;
    // inputActions.Player.Move.Disable();
    // inputActions.Player.Look.Disable(); 
    // inputActions.Player.Trigger.Disable();
    // inputActions.Player.Click.Disable();
    inputActions.UI.Disable();
    inputActions.Player.Disable();
    SetCurrentSelection();
}

    public void SetCurrentSelection () {
        GameObject temp = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(current);
        current = temp;
    }


}