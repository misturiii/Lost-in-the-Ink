using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public GameObject inventoryPanel;      // 物品栏面板引用
    public GameObject sketchBookPanel;     // SketchBook 面板引用
    public StickerController1 stickerController;  // StickerController1 的引用
    public PlayerInput playerInput;        // 引用 PlayerInput 组件

    private bool isUIActive = false;       // 跟踪当前 UI 状态

    void Start()
    {
        // 隐藏所有 UI 面板
        inventoryPanel.SetActive(false);
        sketchBookPanel.SetActive(false);

        // 确保 playerInput 被绑定
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        // 绑定 Trigger 事件
        playerInput.actions["Trigger"].performed += OnToggleUI;
    }

    // 切换 UI 显示和隐藏
    public void OnToggleUI(InputAction.CallbackContext context)
    {
        isUIActive = !isUIActive;
        ToggleUI(isUIActive);
    }

    // 显示或隐藏 UI 面板，并通知 StickerController1 切换 Action Map
    public void ToggleUI(bool isUIActive)
    {
        inventoryPanel.SetActive(isUIActive);
        sketchBookPanel.SetActive(isUIActive);

        if (stickerController != null)
        {
            // 通知 StickerController1 切换 Action Map 和显示/隐藏虚拟光标
            stickerController.ToggleUI(isUIActive);
        }

        // 根据 UI 状态切换 PlayerInput 的 Action Map
        if (isUIActive)
        {
            // 切换到 UI 的 Action Map
            playerInput.SwitchCurrentActionMap("UI");
        }
        else
        {
            // 切换回 Player 的 Action Map
            playerInput.SwitchCurrentActionMap("Player");
        }
    }

}
