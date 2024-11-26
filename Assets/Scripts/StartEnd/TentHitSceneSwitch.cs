using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TentHitSceneSwitch : MonoBehaviour
{
    [Header("Transition Settings")]
    public string tentTag = "end"; // Tag assigned to the circus tent
    public string targetScene = "End"; // Name of the scene to load
    public float fadeDuration = 1.0f; // Duration of the fade-out effect
    public CanvasGroup fadeCanvasGroup; // CanvasGroup for the fade effect

    private bool isTransitioning = false;

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the circus tent
        if (!isTransitioning && other.CompareTag(tentTag))
        {
            StartCoroutine(SwitchScene());
        }
    }

    private IEnumerator SwitchScene()
    {
        isTransitioning = true;

        // Perform fade-out effect
        if (fadeCanvasGroup != null)
        {
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            fadeCanvasGroup.alpha = 1f;
        }

        // Load the next scene
        SceneManager.LoadScene(targetScene);
    }
}
