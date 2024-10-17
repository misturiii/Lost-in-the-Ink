using UnityEngine;

public class Landmark : MonoBehaviour
{
    bool removed = false;
    void OnCollisionEnter (Collision collision) {
        if (!removed && collision.collider.tag == "Ground") {
            Destroy(GetComponent<Rigidbody>());
            removed = true;
        }
    }
}
