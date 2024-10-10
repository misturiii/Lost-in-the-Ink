using UnityEngine;

public class AfterEvents : ColorChangeBehaviour
{
    [SerializeField] TriggerEvent[] events;

    public override void change()
    {
        StartCoroutine(autoColorChange(1));
    }

    public override bool triggerChange()
    {
        if (progress != 0) return false;
        foreach (TriggerEvent ev in events) {
            if (!ev.Finished()) {
                return false;
            }
        }
        return true;
    }
}
