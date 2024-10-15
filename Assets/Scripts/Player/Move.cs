using UnityEngine;

public class Move : MonoBehaviour
{

    [SerializeField, Min(0)] float moveSpeed = 5f;
    Rigidbody rb;

    void Start () {
        rb = GetComponent<Rigidbody>();
    }

    void Update () {
        float scale = moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.localPosition + 
            transform.forward * scale * Input.GetAxis("Vertical") +  
            transform.right * scale * Input.GetAxis("Horizontal"));
    }
}
