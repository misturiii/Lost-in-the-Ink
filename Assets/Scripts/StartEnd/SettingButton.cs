using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour, IPointerEnterHandler
{
    Button button;

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.Select();
    }

    void Start()
    {
        button = GetComponent<Button>();
    }
}
