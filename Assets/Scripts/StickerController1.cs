using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class StickerController1 : MonoBehaviour
{
    public PlayerInput playerInput;
    private bool isDragging = false;
    private RectTransform selectedSticker;

    // 光标控制
    public RectTransform virtualCursor;

    void Start()
    {
        // 确保 PlayerInput 被正确绑定
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        // 绑定 PlayerInput 的 Action
        playerInput.actions["MoveCursor"].performed += OnMoveCursor;
        playerInput.actions["SelectSticker"].performed += OnSelectSticker;

        // 初始化时隐藏虚拟光标
        if (virtualCursor != null)
        {
            virtualCursor.gameObject.SetActive(false);
        }
    }

    // 处理光标移动
    public void OnMoveCursor(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        virtualCursor.anchoredPosition += input * Time.deltaTime * 100f; // 根据输入调整光标位置
    }

    // 处理选中与放下 Sticker 的逻辑
    public void OnSelectSticker(InputAction.CallbackContext context)
    {
        if (playerInput.currentActionMap.name != "UI") return;

        if (selectedSticker == null)
        {
            // 初始化 PointerEventData
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = virtualCursor.anchoredPosition
            };

            // 存储所有 Raycast 的结果
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            // 遍历所有检测到的结果并打印日志
            foreach (RaycastResult result in results)
            {
                Debug.Log($"Detected UI element: {result.gameObject.name}, Tag: {result.gameObject.tag}");

                // 检查是否检测到 UI 中的 Sticker
                if (result.gameObject.CompareTag("PickableItem")) // 使用 "PickableItem" 作为标签判断
                {
                    selectedSticker = result.gameObject.GetComponent<RectTransform>();
                    isDragging = true;
                    Debug.Log("Sticker selected: " + selectedSticker.name);
                    return;
                }
            }
            Debug.Log("No sticker detected under the cursor.");
        }
        else
        {
            // 放下 Sticker
            isDragging = false;
            selectedSticker = null;
            Debug.Log("Sticker released.");
        }
    }


    // 更新虚拟光标的拖拽位置
    void Update()
    {
        if (isDragging && selectedSticker != null)
        {
            // 使用光标的位置更新 Sticker 的位置
            selectedSticker.anchoredPosition = virtualCursor.anchoredPosition;

            // 确保虚拟光标在屏幕范围内
            Vector2 clampedPosition = new Vector2(
                Mathf.Clamp(virtualCursor.anchoredPosition.x, 0, Screen.width),
                Mathf.Clamp(virtualCursor.anchoredPosition.y, 0, Screen.height)
            );
            virtualCursor.anchoredPosition = clampedPosition;
        }
    }


    // 切换 UI 的显示状态（显示/隐藏虚拟光标）
    public void ToggleUI(bool isUIActive)
    {
        if (virtualCursor != null)
        {
            // 仅在 UI 激活时显示虚拟光标
            virtualCursor.gameObject.SetActive(isUIActive);
        }
    }

}
