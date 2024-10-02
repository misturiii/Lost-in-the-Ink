using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject paint;
    [SerializeField] float height, strength;
    [SerializeField] string key;
    [SerializeField] ColorChangeBehaviour behaviour;
    Transform cam;

    void Start () {
        cam = GetComponentInChildren<Camera>().transform;
    }

    void Update () {
        if (!behaviour || behaviour.Changed) {
            if (Input.GetButtonDown(key)) {
                Fire();
            } 
        }
    }

    void Fire () {
        GameObject copy = Instantiate(paint);
        copy.transform.localPosition = transform.localPosition + Vector3.up + transform.forward;
        copy.GetComponent<Rigidbody>().AddForce(height * Vector3.up + (cam.forward + transform.forward) * strength);
    }
}
