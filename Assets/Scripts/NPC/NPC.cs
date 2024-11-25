using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public DialogueObject dialogueObject {get; set;}
    [SerializeField] public string dialogueName;
    QuickOutline outline;
    [SerializeField] GameObject[] EnableAfterInteract;
    [SerializeField] GameObject[] ToggleNPCAfterInteract;
    [SerializeField] TextMeshPro mark;
    
    bool Finsihed = false;
    Vector3 initialRotation = new Vector3(0, -90, 0);

    void Start () {
        if (!outline) {
            SetOutline();
        }
        outline.enabled = false;
        foreach (var s in EnableAfterInteract) {
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
        if (mark) {mark.enabled = false;}
        
    }

    void SetOutline () {
        outline = gameObject.AddComponent<QuickOutline>();
        outline.OutlineColor = FunctionLibrary.LineColor1;
        outline.OutlineMode = QuickOutline.Mode.OutlineVisible;
        outline.OutlineWidth = 10;
        dialogueObject = Resources.Load<DialogueObject>("Dialogue/" + dialogueName);
    }

    void Update() {
        if (tag == "Npc" && dialogueObject && !Finsihed && mark) {
            mark.enabled = true;
            mark.transform.LookAt(Camera.main.transform.position);
        }
        if (name.Contains("Jester")) {
            if (outline.enabled) {
                Vector3 p = Camera.main.transform.position;
                p.y = transform.position.y;
                transform.LookAt(p);
            } else {
                transform.localEulerAngles = initialRotation;
            }
        }
    }

    public DialogueObject Enter () {
        if (!outline) {
            SetOutline();
        }
        if (outline.enabled == false) {
            if (name.Contains("Jester")) {
                JesterAnimation.enter?.Invoke();
            }
            outline.enabled = true;
        }
        return dialogueObject;
    }

    public void Exit () {
        outline.enabled = false;
        if (name.Contains("Jester")) {
            JesterAnimation.exit?.Invoke();
        }
    }

    public void DialogueEnds () {
        if (!Finsihed) {
            foreach (var s in EnableAfterInteract) {
            // Set each sticker to active
                if (s != null) {
                    s.SetActive(true);
                }
            }
            foreach (var obj in ToggleNPCAfterInteract) {
                if (obj.tag == "Npc") {
                    obj.tag = "Untagged";
                } else {
                    obj.tag = "Npc";
                }   
            }
            if (mark) {mark.enabled = false;}
            Finsihed = true;
        }
        JesterAnimation.clap?.Invoke();
    }
}