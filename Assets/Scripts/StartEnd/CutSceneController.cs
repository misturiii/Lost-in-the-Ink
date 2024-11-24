using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    public Image image1; // Assign the first image in the Inspector
    public Image image2; // Assign the second image in the Inspector
    public float displayTime = 3f; // Time each image is displayed
    public string nextSceneName = "GameScene"; // The name of the next scene to load

    private void Start()
    {
        // Start the sequence of images
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        // Show the first image
        image1.gameObject.SetActive(true);
        image2.gameObject.SetActive(false);
        yield return new WaitForSeconds(displayTime);

        // Show the second image
        image1.gameObject.SetActive(false);
        image2.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
