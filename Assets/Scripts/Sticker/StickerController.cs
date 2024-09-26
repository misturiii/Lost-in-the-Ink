using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class StickerController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform stickerRect; // UI 贴纸的 RectTransform
    private CanvasGroup canvasGroup;   // 用于控制 UI 的交互
    private bool isDragging = false;   // 是否在拖动贴纸
    private Vector2 moveInput;         // 存储键盘或手柄的移动输入

    void Start()
    {
        // 获取贴纸的 RectTransform 和 CanvasGroup
        stickerRect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (isDragging)
        {
            // 使用输入来移动贴纸
            MoveSticker();
        }
    }

    // 处理 Move 输入 (WASD 或 左摇杆)
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // 获取输入方向
    }

    // 当玩家按下空格键或者手柄按钮时，开始拖动
    public void OnGrabSticker(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartDragging();
        }
        else if (context.canceled)
        {
            StopDragging();
        }
    }

    // 开始拖动
    private void StartDragging()
    {
        isDragging = true;
        canvasGroup.blocksRaycasts = false; // 让拖动物体能够穿过 UI
    }

    // 停止拖动
    private void StopDragging()
    {
        isDragging = false;
        canvasGroup.blocksRaycasts = true; // 重新启用对 UI 的阻挡检测
    }

    // 移动贴纸的方法
    private void MoveSticker()
    {
        stickerRect.anchoredPosition += moveInput * Time.deltaTime * 500f; // 根据输入方向移动贴纸
    }

    // 处理鼠标点击（开始拖动）
    public void OnPointerDown(PointerEventData eventData)
    {
        StartDragging();
    }

    // 处理鼠标释放（停止拖动）
    public void OnPointerUp(PointerEventData eventData)
    {
        StopDragging();
    }
}
