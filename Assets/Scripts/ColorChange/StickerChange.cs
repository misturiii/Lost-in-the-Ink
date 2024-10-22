using UnityEngine;
using UnityEngine.UI;

public class StickerChange : MonoBehaviour {
    Material mat;
    float duration = 0.5f;

    protected static readonly int 
        durationId = Shader.PropertyToID("_Duration"), 
        startTimeId = Shader.PropertyToID("_StartTime");
    void Start () {
        mat = GetComponent<Image>().material;
        mat.SetFloat(durationId, 0);
    }

    public void Change () {
        mat.SetFloat(durationId, duration);
        mat.SetFloat(startTimeId, Time.time);
    }
}