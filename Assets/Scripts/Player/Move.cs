using UnityEngine;

public class Move : MonoBehaviour
{

    [SerializeField, Min(0)] float moveSpeed = 5f;
    Rigidbody rb;

    void Start () {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        
        rb.MovePosition(transform.localPosition + 
            transform.forward * moveSpeed * Input.GetAxis("Vertical") +  
            transform.right * moveSpeed * Input.GetAxis("Horizontal"));
    }
}
