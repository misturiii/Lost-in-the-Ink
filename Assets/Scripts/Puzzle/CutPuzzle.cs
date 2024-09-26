using UnityEngine;

public class CutPuzzle : MonoBehaviour, Puzzle
{
    [SerializeField] LineRenderer line, dash;
    [SerializeField] Transform scissor;
    [SerializeField] float tolerance = 0.1f, minStep = 0.1f;
    [SerializeField] float moveSpeed = 1f, offset = 1f;
    ViewPuzzle viewPuzzle;
    Vector3 initialPosition;
    Vector3 initialRotation;
    Vector3 prevPosition;
    InputActions inputActions;
    bool finished;

    public float Offset() => offset;

    void Start () {
        prevPosition = initialPosition = scissor.localPosition;
        initialRotation = scissor.localEulerAngles;
        line.positionCount = 2;
        line.SetPosition(0, initialPosition);
        line.SetPosition(1, initialPosition);
        finished = false;

        inputActions = new InputActions();
        inputActions.Puzzle.Enable();
        viewPuzzle = GameObject.FindGameObjectWithTag("Player").GetComponent<ViewPuzzle>();
    }

    void Update () {
        if (!finished) {
            if (inputActions.Puzzle.Click.inProgress) {
                Cut();
            } else {
                Check();
            }
        }
    }

    void Cut () {
        Vector2 input = inputActions.Puzzle.Move.ReadValue<Vector2>();
            float scale = moveSpeed * Time.deltaTime;
            Vector3 direction = Vector3.right * input.x * scale;
            direction += Vector3.up * input.y * scale;
            scissor.localPosition += direction;
            if (direction != Vector3.zero) {
                if (input.y > 0) {
                    scissor.localEulerAngles = -Vector3.forward * Vector3.Angle(-Vector3.right, direction);
                } else {
                    scissor.localEulerAngles = Vector3.forward * Vector3.Angle(-Vector3.right, direction);
                }
            }
            if ((scissor.localPosition - prevPosition).magnitude >= minStep) {
                line.positionCount++;
                prevPosition = scissor.localPosition;
            }
            line.SetPosition(line.positionCount - 1, scissor.localPosition);
    }

    void Check () {
        if (LineMatchDash()) {
            viewPuzzle.StartZoomIn();
            Destroy(scissor.parent.gameObject);
            finished = true;
        } else {
            line.positionCount = 2;
            scissor.localPosition = initialPosition;
            scissor.localEulerAngles = initialRotation;
        }
    }

    bool LineMatchDash () {
        int total = dash.positionCount;
        int index = 0;
        Vector2 a = dash.GetPosition(index);
        Vector2 endpoint = (Vector2)dash.GetPosition((index + 1) % total) - a;
        for (int i = 0; i < line.positionCount; i++) {
            if (!CheckDistance((Vector2)line.GetPosition(i) - a, endpoint)) {
                index++;
                a += endpoint;
                endpoint = (Vector2)dash.GetPosition((index + 1) % total) - a;
                if (!CheckDistance((Vector2)line.GetPosition(i) - a, endpoint)) {
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
