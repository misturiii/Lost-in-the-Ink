using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class StartManager : MonoBehaviour
{
    public AudioSource audioSource;  // Reference to the AudioSource component
    public AudioClip startSound;     // The sound to play when the game starts

    public void StartGame()
    {
        if (audioSource != null && startSound != null)
        {
            audioSource.PlayOneShot(startSound);  // Play the start sound effect
            StartCoroutine(WaitAndStartGame());   // Wait for the sound to finish
        }
        else
        {
            // If no sound is set, load the scene immediately
            SceneManager.LoadScene("level1");
        }
    }

    private IEnumerator WaitAndStartGame()
    {
        // Wait for the duration of the sound before changing the scene
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("level1");  // Load the next scene after the sound plays
    }

    public void OpenGuide()
    {
        Debug.Log("Guide button clicked!");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit button clicked!");
    }
}