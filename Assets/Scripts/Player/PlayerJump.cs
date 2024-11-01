using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 5f; 
    private Rigidbody rb;
    private bool isGrounded = true;
    private InputActions inputActions;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 初始化输入动作
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += OnJump; 
    }

    // 跳跃事件
    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; 
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        isGrounded = true; 
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.performed -= OnJump;
    }
}
