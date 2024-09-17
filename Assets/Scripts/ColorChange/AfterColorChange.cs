using UnityEngine;

public class AfterColorChange : ColorChangeBehaviour
{
    [SerializeField] ColorChangeBehaviour[] behaviours;

    public override void change()
    {
        StartCoroutine(autoColorChange(1));
    }

    public override bool triggerChange()
    { 
        if (progress != 0) return false;
        foreach (ColorChangeBehaviour behaviour in behaviours) {
            if (!behaviour.Changed) {
                return false;
            }
        }
        return true;
    }
}
