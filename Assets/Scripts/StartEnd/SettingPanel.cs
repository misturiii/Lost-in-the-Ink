using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    Button button;
    EventSystem eventSystem;
    // Start is called before the first frame update
    bool clicked = false;
    void Start()
    {
        button = GetComponent<Button>();
        eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (eventSystem.currentSelectedGameObject == gameObject) {
            if (!clicked) {
                button.onClick.Invoke();
                clicked = true;
            }
        } else {
            clicked = false;
        }
    }
}
