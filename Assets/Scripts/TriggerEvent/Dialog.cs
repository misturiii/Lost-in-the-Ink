using TMPro;
using UnityEngine;

public class Dialog : TriggerEvent
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshPro questionMark;
    [SerializeField] string[] dialogs;
    [SerializeField] string key = "Fire3";
    int index = 0;
    GameObject panel;
    Move playerMove;
    Rotate playerRotate;

    public override bool Finished()
    {
        return index > dialogs.Length;
    }

    void Start()
    {
        panel = text.transform.parent.gameObject;
        panel.SetActive(false);
        playerMove = player.GetComponent<Move>();
        playerRotate = player.GetComponentInChildren<Rotate>();
    }

    void Update()
    {
        if (FindPlayer() && Input.GetButtonDown(key)) {
            if (index < dialogs.Length) {
                if (index == 0) {
                    SetDialogMode(true);
                }
                text.SetText(dialogs[index++]);
            } else {
                SetDialogMode(false);
                questionMark.enabled = false;
                index++;
            }
        }
    }

    void SetDialogMode (bool set) {
        panel.SetActive(set);
        playerMove.enabled = !set;
        playerRotate.enabled = !set;
    }
}
