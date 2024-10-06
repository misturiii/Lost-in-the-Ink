using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f, rotateSpeed = 100, maxField = 50;
    Rigidbody rb;
    Transform mainCamera;
    float xRotation;
    InputActions inputActions;

    void Start () {
        rb = GetComponent<Rigidbody>();
        mainCamera = GetComponentInChildren<Camera>().transform;

        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.Enable();
    }

    void Update () {
        Move(inputActions.Player.Move.ReadValue<Vector2>());
        Rotate(inputActions.Player.Look.ReadValue<Vector2>());
    }

    void Move (Vector2 input) {
        float scale = moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.localPosition + 
            transform.forward * scale * input.y +  
            transform.right * scale * input.x);
    }

    void Rotate (Vector2 input) {
        float scale = Time.deltaTime * rotateSpeed;
        xRotation -= input.y * scale;
        xRotation = Mathf.Clamp(xRotation, -maxField, maxField);
        mainCamera.localEulerAngles = Vector3.right * xRotation;
        transform.Rotate(Vector3.up * input.x * scale);
    }

}
