using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceManager : MonoBehaviour
{
    private string inputDevice = ""; 
    public void DetectInputDevice()
    {
        
        if (Keyboard.current.anyKey.isPressed)
        {
            inputDevice = "keyboard";
        }
        
        else if (Mouse.current.leftButton.isPressed)
        {
            inputDevice = "keyboard"; 
        }
        
        else if (Gamepad.current != null && Gamepad.current.buttonSouth.isPressed)
        {
            inputDevice = "controller";
        }

        PlayerPrefs.SetString("InputDevice", inputDevice);
        Debug.Log("Detected input device: " + inputDevice);
    }
}
