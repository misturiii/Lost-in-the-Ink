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
            Vector2 viewPos = cam.WorldToViewportPoint(transform.position);
            if ((-0.1 > viewPos.x || viewPos.x > 1.1) && (-0.1 > viewPos.y || viewPos.y > 1.1)) {
                Debug.Log($"position is {viewPos}, NPC removed");
                next.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}
