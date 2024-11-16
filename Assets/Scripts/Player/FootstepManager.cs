
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    private AudioSource audioSource;
    public Rigidbody playerRigidbody;  // Reference to the player's Rigidbody
    public float movementThreshold = 0.05f;  // Threshold to detect player movement
    public float stepCooldown = 0.5f;  // Time between footsteps
    private float nextStepTime = 0f;   // Timer to control footstep intervals
    private Vector3 lastPosition;

    private AudioClip[] currentFootstepArray;  // Currently active footstep sound array based on surface

    public GameObject floorObject;  // Reference to the parent floor object (assign in the Inspector)
    private Surface currentSurface; // Track the current surface the player is on

    void Start()
    {
        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();
        lastPosition = playerRigidbody.position;

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on FootstepManager!");
        }

        // Default to an empty array to avoid null reference errors
        currentFootstepArray = new AudioClip[0];
    }

    void Update()
    {
        // Check if the player is moving
        if (IsPlayerMoving())
        {
            // If it's time for the next step, play a random footstep sound
            if (Time.time >= nextStepTime)
            {
                PlayRandomFootstep();
                nextStepTime = Time.time + stepCooldown;  // Set the time for the next step
            }
        }

        // Continuously check if the player has entered a new surface
        CheckForSurfaceChange();
    }

    // Function to check if the player is moving based on Rigidbody velocity
    bool IsPlayerMoving()
    {
        // Calculate the distance moved since the last frame
        float distanceMoved = Vector3.Distance(playerRigidbody.position, lastPosition);

        // Update lastPosition to the current position for the next frame
        lastPosition = playerRigidbody.position;

        // Check if the distance moved is greater than a small threshold (e.g., 0.01f)
        return distanceMoved > movementThreshold;
    }

    // Function to play a random footstep sound based on the current surface
    void PlayRandomFootstep()
    {
        if (currentFootstepArray.Length > 0)
        {
            // Randomly select a clip from the current footstep array
            int randomIndex = Random.Range(0, currentFootstepArray.Length);

            AudioClip randomClip = currentFootstepArray[randomIndex];

            // Play the sound using PlayOneShot so that each footstep is distinct
            if (randomClip != null)
            {
                audioSource.PlayOneShot(randomClip);
            }
            else
            {
                Debug.LogWarning("Footstep clip is null at index: " + randomIndex);
            }
        }
    }

    // Continuously checks if the player has changed surfaces
    void CheckForSurfaceChange()
    {
        // Cast a ray downward from the player's position to detect the surface below
        RaycastHit hit;
        if (Physics.Raycast(playerRigidbody.position, Vector3.down, out hit, 2f))
        {
            Surface surface = hit.collider.GetComponent<Surface>();

            if (surface != null && surface != currentSurface)
            {
                // Update the current surface and change the footstep sounds
                currentSurface = surface;
                currentFootstepArray = surface.footstepClips;

                // Debug log to show the new surface and footstep sounds
                Debug.Log("Player is now on surface: " + hit.collider.name);
                Debug.Log("Footstep sounds for this surface:");
                foreach (var clip in currentFootstepArray)
                {
                    if (clip != null)
                    {
                        Debug.Log(clip.name);  // Print the name of each footstep sound clip
                    }
                    else
                    {
                        Debug.Log("Null clip found in the footstep array.");
                    }
                }
            }
        }
        else
        {
            // If no surface is detected, reset the footstep sounds
            if (currentSurface != null)
            {
                Debug.Log("Player left surface: " + currentSurface.name);
                currentSurface = null;
                currentFootstepArray = new AudioClip[0];
            }
        }
    }
}
