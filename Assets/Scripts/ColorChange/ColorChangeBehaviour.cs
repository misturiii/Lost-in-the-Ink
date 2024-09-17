using System.Collections;
using UnityEngine;

public abstract class ColorChangeBehaviour : MonoBehaviour
{
    [SerializeField] protected float duration = 2;
    protected Material[] mats;
    protected float progress = 0;
    public bool Changed => progress >= 1;

    protected static readonly int progressId = Shader.PropertyToID("_Progress");

    void Start () {
        mats = new Material[transform.childCount];
        int j = 0;
        for (int i = 0; i < mats.Length; i++) {
            Renderer r = transform.GetChild(i).GetComponent<Renderer>();
            if (r) {
                mats[j++] = r.material;
            }
        }
    }

    protected void Update () {
        if (triggerChange()) {
            change();
        }
    }

    public abstract bool triggerChange ();

    public abstract void change ();

    protected IEnumerator autoColorChange (float end) {
        while (progress < end) {
            progress += Time.deltaTime / duration;
            foreach (Material mat in mats) {
                if (mat) {
                    mat.SetFloat(progressId, progress);
                } else {
                    break;
                }                
            }
            yield return null;
        }
    }
}
