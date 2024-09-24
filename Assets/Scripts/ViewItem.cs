using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ViewItem : MonoBehaviour
{
    [SerializeField] float offset = 1;
    [SerializeField] float distance = 1, duration = 2;
    [SerializeField] Transform sticker;
    [SerializeField] Volume volume;
    Camera view;
    Player player;
    Vector3 curPosition;

    void Start()
    {
        view = GameObject.FindGameObjectWithTag("ViewCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        curPosition = transform.position;
    }

    protected void Update () {
        if (Input.GetButtonDown("Jump") && FindPlayer()) {
            if (view.enabled) {
                view.enabled = false;
                player.EnableMoveAndRotate(true);
                volume.enabled = false;
                
            } else {
                SetViewCamera();
                view.enabled = true;
                player.EnableMoveAndRotate(false);
                volume.enabled = true;
            }
        }
    }

    void SetViewCamera () {
        view.transform.localEulerAngles = transform.eulerAngles;
        view.transform.localPosition = curPosition + transform.forward * offset;
    }

    bool FindPlayer () {
        Vector3 playerPosition = player.transform.localPosition;
        return (playerPosition - curPosition).magnitude <= distance;
    }

    public void StartZoomIn () {
        StartCoroutine(ZoomIn());
    }

    IEnumerator ZoomIn () {
        Vector3 initialPosition = sticker.localPosition;
        float i = 0; 
        while (i < 1) {
            i += Time.deltaTime / duration;
            sticker.localPosition = Vector3.Lerp(initialPosition, Vector3.zero, Mathf.Min(i, 1));
            Debug.Log(i);
            yield return null;
        }
    }
}
