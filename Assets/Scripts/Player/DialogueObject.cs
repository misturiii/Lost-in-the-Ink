using System;
using UnityEngine;

[Serializable]
public struct DialogueLine {
    [SerializeField] public string speaker;
    [SerializeField] public string[] lines;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "DialogueSystem/Dialogue")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] DialogueLine[] dialogues;
    int index = 0;
    int lineIndex = 0;

    public (string, string) CurrentLine () {
        (string, string) ret = (dialogues[index].speaker, dialogues[index].lines[lineIndex]);
        if (LineIsOver()) {
            index++;
            lineIndex = 0;
        } else {
            lineIndex++;
        }
        return ret;
    }

    public bool IsOver () => index == dialogues.Length;

    bool LineIsOver () => dialogues[index].lines.Length == lineIndex + 1;

    public void Reset () => index = lineIndex = 0;
}
