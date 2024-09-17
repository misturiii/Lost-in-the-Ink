using System.Collections;
using UnityEngine;

public class Paint : MonoBehaviour
{
    [SerializeField] ParticleSystem splashParticleSystem;
    void Update () {
        if (transform.localPosition.y < -10) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter (Collision collision) {
        if (collision.collider.tag == "Monster") {
            collision.gameObject.GetComponentInParent<GetHit>().GetHitByPaint();
        }
        StartCoroutine(splash());
        Destroy(gameObject);
    }

    IEnumerator splash () {
        ParticleSystem copy = Instantiate(splashParticleSystem);
        copy.transform.localPosition = transform.localPosition;
        copy.Play();
        yield return null;
    }
}
