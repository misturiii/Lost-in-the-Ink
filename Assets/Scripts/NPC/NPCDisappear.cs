using UnityEngine;

public class NPCDisappear : MonoBehaviour
{
    Camera cam;
    NPC npc;
    [SerializeField] GameObject next;
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        npc = GetComponent<NPC>();
        next.SetActive(false);
    }

    void Update()
    {
        if (npc.dialogueObject.notFirstTime) {
            Vector2 viewPos = cam.WorldToViewportPoint(transform.localPosition);
            if (0 > viewPos.x || viewPos.x > 1 || 0 > viewPos.y || viewPos.y > 1) {
                next.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}
