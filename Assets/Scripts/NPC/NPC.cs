using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueObject dialogueObject {get; set;}
    [SerializeField] string dialogueName;
    QuickOutline outline;
    Color outlineColor = new Color(0.7f, 0.9f, 1f);
    [SerializeField] GameObject sticker;

    void Start () {
        dialogueObject = Resources.Load<DialogueObject>(dialogueName);
        outline = gameObject.AddComponent<QuickOutline>();
        outline.OutlineColor = outlineColor;
        outline.OutlineMode = QuickOutline.Mode.OutlineVisible;
        outline.OutlineWidth = 10;
        outline.enabled = false;
        if (sticker) {
            sticker.SetActive(false);
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
        if (sticker) {
            sticker.SetActive(true);
        }
    }
}
