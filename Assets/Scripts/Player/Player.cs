using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f, rotateSpeed = 100, maxField = 50;
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

    void Update () {
        Move(inputActions.Player.Move.ReadValue<Vector2>());
        Rotate(inputActions.Player.Look.ReadValue<Vector2>());

        if (transform.localPosition.y < -5) {
            Respawn();
        }
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

    void Respawn () {
        rb.isKinematic = true;
        Vector3 p = transform.localPosition;
        p.y = initialHeight;
        transform.localPosition = p;
        rb.isKinematic = false;
    }

}
