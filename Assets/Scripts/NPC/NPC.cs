using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public DialogueObject dialogueObject { get; set; }
    [SerializeField] public string dialogueName;
    [SerializeField] private string[] randomDialogueNames;
    private List<string> randomDialogueNameList;
    QuickOutline outline;
    [SerializeField] GameObject[] EnableAfterInteract;
    [SerializeField] GameObject[] ToggleNPCAfterInteract;
    [SerializeField] TextMeshPro mark;
    
    bool Finsihed = false;
    Vector3 initialRotation = new Vector3(0, -90, 0);

    void Start () {
        // Initialize the random dialogue list properly if not set
        if (randomDialogueNames == null || randomDialogueNames.Length == 0) {
            randomDialogueNames = new string[0]; // Ensure it's not null
        }
        randomDialogueNameList = new List<string>(randomDialogueNames);

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
        if (mark) { mark.enabled = false; }
    }

    void SetOutline () {
        outline = gameObject.AddComponent<QuickOutline>();
        outline.OutlineColor = FunctionLibrary.LineColor1;
        outline.OutlineMode = QuickOutline.Mode.OutlineVisible;
        outline.OutlineWidth = 10;
        if (dialogueName.Contains("Hint") && randomDialogueNames.Length > 0) {
            string randomDialogueName = randomDialogueNames[UnityEngine.Random.Range(0, randomDialogueNames.Length)];
            // Ensure dialogueName is updated with the random one
            dialogueName = randomDialogueName;
        }
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

        if (dialogueName.Contains("Hint")) {
            string randomDialogueName = randomDialogueNames[UnityEngine.Random.Range(0, randomDialogueNames.Length)];
            dialogueName = randomDialogueName;  // Update the dialogue name with the random one
        }

        // Ensure dialogueName is set
        if (string.IsNullOrEmpty(dialogueName)) {
            Debug.LogError("Dialogue name is not set on NPC: " + name);
            return null;
        }

        // Load the dialogue object based on the updated dialogueName
        dialogueObject = Resources.Load<DialogueObject>("Dialogue/" + dialogueName);

        if (dialogueObject == null) {
            Debug.LogError("Failed to load DialogueObject for NPC: " + name + " with dialogueName: " + dialogueName);
            return null;
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
            if (mark) { mark.enabled = false; }
            Finsihed = true;
        }
        JesterAnimation.clap?.Invoke();
    }

    // Method to add a hint directly to the NPC
    public void AddHint(string stickerName)
    {
        string hint = ConvertStickerToHint(stickerName);
        if (!string.IsNullOrEmpty(hint) && !randomDialogueNameList.Contains(hint))
        {
            randomDialogueNameList.Add(hint);
            randomDialogueNames = randomDialogueNameList.ToArray();

            // Optionally, you can trigger an update in the Inspector (via the Unity Editor)
            UnityEditor.EditorUtility.SetDirty(this);
        }
        else
        {
            Debug.LogWarning($"Failed to add hint for sticker '{stickerName}' to NPC '{name}'");
        }
    }

    public void RemoveHint(string stickerName)
    {
        string hint = ConvertStickerToHint(stickerName);
        if (!string.IsNullOrEmpty(hint) && randomDialogueNameList.Contains(hint))
        {
            randomDialogueNameList.Remove(hint);
            randomDialogueNames = randomDialogueNameList.ToArray();

            // Optionally, trigger an update in the Inspector (via Unity Editor)
            UnityEditor.EditorUtility.SetDirty(this);
        }
        else
        {
            Debug.LogWarning($"Failed to remove hint for sticker '{stickerName}' from NPC '{name}'");
        }
    }


    // Convert a sticker name to a corresponding hint
    private string ConvertStickerToHint(string stickerName)
    {
        switch (stickerName)
        {
            case "Balloon pieces":
                return "Hint-Balloon";
            case "Balloon cart-incomplete":
                return "Hint-Cart";
            case "Bush":
                return "Hint-Bush";
            case "Coin":
                return "Hint-Coin";
            case "Incomplete ferris wheel":
                return "Hint-Ferris";
            case "Ice cream cart":
                return "Hint-Ice Cream";
            case "Tree":
                return "Hint-Tree";
            case "Circus":
                return "Hint-Circus";
            default:
                return null;
        }
    }
}
