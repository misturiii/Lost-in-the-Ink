using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
    public AudioSource audioSource;  // Reference to the AudioSource component

    // Method to play a specific sound clip when a button is clicked
    public void PlaySoundEffect(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;  // Assign the provided AudioClip to the AudioSource
            audioSource.Play();       // Play the sound
        }
        else
        {
            Debug.LogWarning("No AudioSource or AudioClip assigned.");
        }
    }
}
