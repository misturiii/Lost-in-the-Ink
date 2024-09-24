using UnityEngine;

public class CutPuzzle : MonoBehaviour
{
    [SerializeField] LineRenderer lr;
    [SerializeField] LineRenderer dash;
    [SerializeField] float tolerance = 0.1f, minStep = 0.1f;
    [SerializeField] float moveSpeed = 1f;
    ViewItem item;
    Vector3 initialPosition;
    Vector3 initialRotation;
    Vector3 prevPosition;

    void Start () {
        prevPosition = initialPosition = transform.localPosition;
        initialRotation = transform.localEulerAngles;
        lr.positionCount = 2;
        lr.SetPosition(0, initialPosition);
        lr.SetPosition(1, initialPosition);
        item = transform.parent.parent.GetComponent<ViewItem>();
    }

    void Update () {
        if (Input.GetButton("Fire1")) {
            float scale = moveSpeed * Time.deltaTime;
            float v = Input.GetAxis("Vertical");
            Vector3 direction = Vector3.right * Input.GetAxis("Horizontal") * scale;
            direction += Vector3.up * v * scale;
            transform.localPosition += direction;
            if (direction != Vector3.zero) {
                if (v > 0) {
                    transform.localEulerAngles = -Vector3.forward * Vector3.Angle(-Vector3.right, direction);
                } else {
                    transform.localEulerAngles = Vector3.forward * Vector3.Angle(-Vector3.right, direction);
                }
            }
            if ((transform.localPosition - prevPosition).magnitude >= minStep) {
                lr.positionCount++;
                prevPosition = transform.localPosition;
            }
            lr.SetPosition(lr.positionCount - 1, transform.localPosition);
        }
        if (Input.GetButtonUp("Fire1")) {
            if (LineMatchDash()) {
                item.StartZoomIn();
                Destroy(transform.parent.gameObject);
               

            } else {
                lr.positionCount = 2;
                transform.localPosition = initialPosition;
                transform.localEulerAngles = initialRotation;
            }
        }
    }

    bool LineMatchDash () {
        int total = dash.positionCount;
        int index = 0;
        Vector2 a = dash.GetPosition(index);
        Vector2 endpoint = (Vector2)dash.GetPosition((index + 1) % total) - a;
        for (int i = 0; i < lr.positionCount; i++) {
            if (!CheckDistance((Vector2)lr.GetPosition(i) - a, endpoint)) {
                index++;
                a += endpoint;
                endpoint = (Vector2)dash.GetPosition((index + 1) % total) - a;
                if (!CheckDistance((Vector2)lr.GetPosition(i) - a, endpoint)) {
                    return false;
                } 
            }
        }
        return index >= total - 1;
    }

    bool CheckDistance(Vector2 point, Vector2 endpoint) {

        float angle = Mathf.Atan2(point.y, point.x) - Mathf.Atan2(endpoint.y, endpoint.x);
        return Mathf.Abs(Mathf.Sin(angle) * point.magnitude) <= tolerance;
    }
}
