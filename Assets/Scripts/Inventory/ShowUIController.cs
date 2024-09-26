using UnityEngine;
using UnityEngine.UI;

public class ShowUIController : MonoBehaviour
{
    public GameObject sketchBookPanel; // 拖入你的 SketchBookPanel
    public GameObject inventoryPanel; // 拖入你的 Inventory_Panel
    public Button showUIButton; // 拖入你的按钮

    void Start()
    {
        // 开始时隐藏UI元素
        sketchBookPanel.SetActive(false);
        inventoryPanel.SetActive(false);

        // 添加按钮点击事件监听器
        showUIButton.onClick.AddListener(ShowUI);
    }

    // 当按钮点击时，显示UI
    void ShowUI()
    {
        sketchBookPanel.SetActive(true);
        inventoryPanel.SetActive(true);
        showUIButton.gameObject.SetActive(false); // 显示UI后隐藏按钮
    }
}

