using System.Collections;
using UnityEngine;

public class MonsterAppear : TriggerEvent
{
    [SerializeField] float targetHeight;
    [SerializeField] float duration;
    [SerializeField] MonsterMove move;
    float intitialHeight;

    public override bool Finished()
    {
        return false;
    }

    void Update () {
        if (FindPlayer()) {
            StartCoroutine(MoveUp());
            enabled = false;
        }
    }

    IEnumerator MoveUp () {
        intitialHeight = transform.localPosition.y;
        for (float i = 0; i < duration; i += Time.deltaTime) {
            Vector3 p = transform.localPosition;
            p.y = Mathf.Lerp(intitialHeight, targetHeight, i / duration);
            transform.localPosition = p;
            yield return null;
        }
        move.enabled = true;
    }

}