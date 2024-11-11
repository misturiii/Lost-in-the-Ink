using UnityEngine;

public enum StickerAudio {remove, drop, cam, rubik, trash}
public class StickerAudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    AudioSource audioSource;

    void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Play (StickerAudio audio) {
        audioSource.PlayOneShot(clips[(int)audio]);
    }
}
