using System.Collections;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    [SerializeField] float duration = 5f;
    protected Material mat;
    protected float progress = 0;

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

    void OnCollisionStay (Collision collision) {
        if (progress == 0 && collision.collider.tag == "Ground") {
            StartCoroutine(AutoColorChange());
        }
    }
}
