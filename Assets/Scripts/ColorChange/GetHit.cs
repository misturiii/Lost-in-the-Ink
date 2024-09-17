using UnityEngine;

public class GetHit : ColorChangeBehaviour
{
    [SerializeField] int maxhealth;
    bool hited = false;
    float progressFactor, curMax;

    void Awake () {
        progressFactor = 1.0f / maxhealth;
    }

    public override void change()
    {
        curMax += progressFactor;
        StartCoroutine(autoColorChange(curMax));
    }

    public override bool triggerChange()
    {
        if (progress >= 1) {return false;}
        if (hited) {
            hited = false;
            return true;
        } else {
            return false;
        }
    }

    public void GetHitByPaint () {
        hited = true;
    }
}
