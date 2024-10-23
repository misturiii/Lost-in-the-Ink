using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingSlider : MonoBehaviour
{
    EventSystem eventSystem;
    InputAction adjustAction;
    Slider slider;
    float speed = 0.01f;
    void Start()
    {
        adjustAction = FindObjectOfType<InputActionManager>().inputActions.Global.Navigate;
        eventSystem = EventSystem.current;
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (adjustAction.IsInProgress() && eventSystem.currentSelectedGameObject == gameObject) {
            slider.value = Mathf.Clamp(slider.value + adjustAction.ReadValue<Vector2>().x * speed, 0, 1);
        }
    }
}
