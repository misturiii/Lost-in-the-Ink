using TMPro;
using UnityEngine;

public class DeviceGuideManager : MonoBehaviour
{

    public GameObject keyboardImage;
    public GameObject controllerImage;
    public GameObject controllerImage2;
    public GameObject controllerImage3;
    public GameObject controllerImage4;
    public GameObject controllerImage5;
    public GameObject keyboardText;
    public GameObject controllerText;
    void Start()
    {
        
        string inputDevice = PlayerPrefs.GetString("InputDevice", "keyboard");

        
        if (inputDevice == "keyboard")
        {
            ShowKeyboardGuide();
        }
        else if (inputDevice == "controller")
        {
            ShowControllerGuide();
        }
    }

    
    void ShowKeyboardGuide()
    {
        if (keyboardImage != null)
        {
            keyboardImage.SetActive(true);
        }
        if (keyboardText != null)
        {
            keyboardText.SetActive(true);
        }
        if (controllerImage != null)
        {
            controllerImage.SetActive(false);
        }
        if (controllerImage2 != null)
        {
            controllerImage2.SetActive(false);
        }
        if (controllerImage3 != null)
        {
            controllerImage3.SetActive(false);
        }
        if (controllerImage4 != null)
        {
            controllerImage4.SetActive(false);
        }
        if (controllerImage5 != null)
        {
            controllerImage5.SetActive(false);
        }
        if (controllerText != null)
        {
            controllerText.SetActive(false);
        }

    }

    
    void ShowControllerGuide()
    {
        if (controllerImage != null)
        {
            controllerImage.SetActive(true);
        }
        if (controllerImage2 != null)
        {
            controllerImage2.SetActive(true);
        }
        if (controllerImage3 != null)
        {
            controllerImage3.SetActive(true);
        }
        if (controllerImage4 != null)
        {
            controllerImage4.SetActive(true);
        }
        if (controllerImage5 != null)
        {
            controllerImage5.SetActive(true);
        }
        // 显示手柄的 Guide 文本（如果它存在）
        if (controllerText != null)
        {
            controllerText.SetActive(true);
        }

        // 隐藏键盘的 Guide 图片和文本
        if (keyboardImage != null)
        {
            keyboardImage.SetActive(false);
        }
        if (keyboardText != null)
        {
            keyboardText.SetActive(false);
        }
    }
}
