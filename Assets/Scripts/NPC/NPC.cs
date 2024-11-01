using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueObject dialogueObject {get; set;}
    [SerializeField] string dialogueName;
    QuickOutline outline;
    [SerializeField] GameObject[] sticker;
    [SerializeField] GameObject[] objects;
    [SerializeField] NPC next;

    void Start () {
        dialogueObject = Resources.Load<DialogueObject>("Dialogue/" + dialogueName);
        outline = gameObject.AddComponent<QuickOutline>();
        outline.OutlineColor = FunctionLibrary.LineColor1;
        outline.OutlineMode = QuickOutline.Mode.OutlineVisible;
        outline.OutlineWidth = 10;
        outline.enabled = false;
        foreach (var s in sticker) {
            // Set each sticker to active
            s?.SetActive(false);
        }

        GameObject[] npcs = GameObject.FindGameObjectsWithTag("Npc");
        foreach (GameObject npc in npcs) {
            if (npc != gameObject && npc.name == name) {
                Destroy(npc);
                return;
            }
        }
    }

    public DialogueObject Enter () {
        outline.enabled = true;
        return dialogueObject;
    }

    public void Exit () {
        outline.enabled = false;
    }

    public void DialogueEnds () {
        foreach (var s in sticker) {
            // Set each sticker to active
            if (s != null) {
                s.SetActive(true);
            }
        }
        foreach (var obj in objects) {
            obj.tag = "Npc";
        }
        if (next) {
            next.enabled = true;
        }
    }
}