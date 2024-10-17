using System.Collections;
using UnityEngine;

public class ColorChange1 : MonoBehaviour
{
    [SerializeField] float duration = 5f;
    protected Material mat;
    public float progress = 0;
    [SerializeField] ColorChange target;

    protected static readonly int progressId = Shader.PropertyToID("_Progress");

    void Start () {
        mat = GetComponent<Renderer>().material;
    }

    protected IEnumerator AutoColorChange () {
        while (progress < 1) {
            progress += Time.deltaTime / duration;
            mat.SetFloat(progressId, progress);
            yield return null;
        }
    }

    void Update() {
        if (progress == 0 && target.progress > 0) {
            StartCoroutine(AutoColorChange());
        }
    }
}
