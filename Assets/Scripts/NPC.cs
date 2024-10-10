using UnityEngine;

public class NPC : MonoBehaviour
{
    DialogueObject dialogueObject {get; set;}
    [SerializeField] string dialogueName;
    QuickOutline outline;
    Color outlineColor = new Color(0.7f, 0.9f, 1f);

    void Start () {
        dialogueObject = Resources.Load<DialogueObject>(dialogueName);
        outline = gameObject.AddComponent<QuickOutline>();
        outline.OutlineColor = outlineColor;
        outline.OutlineMode = QuickOutline.Mode.OutlineVisible;
        outline.OutlineWidth = 10;
        outline.enabled = false;
    }

    public DialogueObject Enter () {
        outline.enabled = true;
        return dialogueObject;
    }

    public void Exit () {
        outline.enabled = false;
    }
}
