using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MenuToggle : MonoBehaviour
{
    public GameObject menuUI;  // Reference to the UI panel for the menu
    private bool isPaused = false;
    InputActionManager inputActionManager;
    [SerializeField] GameObject current;
    bool isPlayerActive;

    void Start()
    {
        inputActionManager = FindObjectOfType<InputActionManager>();
        inputActionManager.inputActions.Global.Quit.performed += Toggle;
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
        
        inputActionManager.SetPlayerActive(isPlayerActive);
        isPaused = false;
        SetCurrentSelection();
    }

    public void PauseGame()
{

    menuUI.SetActive(true);  // Show the menu (this ensures the menu is visible)
    // Time.timeScale = 0f;     // Pause the game
    isPaused = true;
    isPlayerActive = inputActionManager.inputActions.Player.enabled;
    inputActionManager.SetAllActive(false);
    SetCurrentSelection();
}

    public void SetCurrentSelection () {
        GameObject temp = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(current);
        current = temp;
    }


}