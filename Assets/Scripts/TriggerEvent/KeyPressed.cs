using UnityEngine;

public class KeyPressed : TriggerEvent
{
    [SerializeField] string key = "Fire1";

    public override bool Finished() {
        if (FindPlayer()) {
            return Input.GetButtonDown(key);
        } else {
            return false;
        }
    }
}
