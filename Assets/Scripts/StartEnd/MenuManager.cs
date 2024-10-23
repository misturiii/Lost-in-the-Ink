using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;  // Required for IEnumerator and coroutines

public class MenuManager : MonoBehaviour
{
    InputActions inputActions;
    public GameObject guidePage;
    public GameObject settingPage;
    public GameObject menuPage;

    public AudioSource audioSource;  // AudioSource to play sounds
    public AudioClip restartSound;   // The sound to play on restart

    void Start()
    {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
    }

    // Restart button logic (Restart the current scene)
    public void RestartGame()
    {
        
        // Check if there is an audio clip to play
        if (audioSource != null && restartSound != null)
        {
            audioSource.PlayOneShot(restartSound);  // Play the restart sound effect
            StartCoroutine(WaitAndRestart());      // Wait for the sound to finish before restarting
        }
        else
        {
            // If no sound is set, restart the scene immediately
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        Debug.Log("Restart is pressed");
    }

    private IEnumerator WaitAndRestart()
    {
        // Wait for the duration of the sound before restarting the scene
        yield return new WaitForSeconds(1f);
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);  // Restart the scene after the sound finishes
    }

    public void OpenSetting()
    {
        settingPage.SetActive(true); 
        guidePage.SetActive(false);
    }

    // Guide button logic (Open Guide)
    public void OpenGuide()
    {
        settingPage.SetActive(false);
        guidePage.SetActive(true);   // Hide the guide page
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited");
    }
}