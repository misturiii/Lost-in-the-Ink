using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    public PlayerInput playerInput;  // 绑定到 PlayerInput 组件
    public float cursorSpeed = 1000f;  // 光标移动速度
    private Vector2 moveInput;  // 存储手柄输入

    void Start()
    {
        // 确保我们已经绑定了 PlayerInput 组件
        playerInput = GetComponent<PlayerInput>();
    }

    // 当 MoveCursor 被触发时，调用这个方法
    public void OnMoveCursor(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();  // 读取输入值
    }

    void Update()
    {
        // 根据手柄输入值移动鼠标光标
        Vector3 cursorMovement = new Vector3(moveInput.x, moveInput.y, 0) * cursorSpeed * Time.deltaTime;
        Vector3 newPosition = Input.mousePosition + cursorMovement;

        // 限制光标位置，避免光标移出屏幕
        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        // 移动鼠标光标到新位置
        Cursor.lockState = CursorLockMode.None;  // 确保光标没有被锁定
        Cursor.SetCursor(null, newPosition, CursorMode.Auto);
    }
}
