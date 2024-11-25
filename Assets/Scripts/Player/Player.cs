using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.2f, rotateSpeed = 2f, maxField = 50, numIteration = 20;
    Rigidbody rb;
    Transform mainCamera;
    float xRotation;
    InputActions inputActions;
    float initialHeight;

    void Start () {
        rb = GetComponent<Rigidbody>();
        mainCamera = GetComponentInChildren<Camera>().transform;

        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.Enable();
        initialHeight = transform.localPosition.y;
    }

    void FixedUpdate () {
        Move(inputActions.Player.Move.ReadValue<Vector2>());
        Rotate(inputActions.Player.Look.ReadValue<Vector2>());

        if (transform.localPosition.y < -5) {
            Respawn();
        }
    }

    void Move (Vector2 input) {
        for (int i = 0; i < numIteration; i++) {
            rb.MovePosition(transform.localPosition + 
                transform.forward * moveSpeed * input.y +  
                transform.right * moveSpeed * input.x);
        }
    }

    void Rotate (Vector2 input) {
        xRotation -= input.y * rotateSpeed;
        xRotation = Mathf.Clamp(xRotation, -maxField, maxField);
        mainCamera.localEulerAngles = Vector3.right * xRotation;
        transform.Rotate(Vector3.up * input.x * rotateSpeed);
    }

    void Respawn () {
        rb.isKinematic = true;
        Vector3 p = transform.localPosition;
        p.y = initialHeight;
        transform.localPosition = p;
        rb.isKinematic = false;
    }

    public void UpdateSettings(float newMoveSpeed, float newRotateSpeed) {
        moveSpeed = newMoveSpeed;
        rotateSpeed = newRotateSpeed;
    }

}
