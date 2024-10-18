using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    public AudioClip[] footstepClips;  // Array to hold footstep sounds
    private AudioSource audioSource;
    public Rigidbody playerRigidbody;  // Reference to the player's Rigidbody
    public float movementThreshold = 0.1f;  // Threshold to detect player movement
    public float stepCooldown = 0.5f;  // Time between footsteps
    private float nextStepTime = 0f;   // Timer to control footstep intervals
    private Vector3 lastPosition; 

    void Start()
    {
        // Make sure AudioSource is correctly initialized
        audioSource = GetComponent<AudioSource>();
        lastPosition = playerRigidbody.position;

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on FootstepManager!");
        }
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
    }

    // Function to check if the player is moving based on Rigidbody velocity
  bool IsPlayerMoving()
{
    // Calculate the distance moved since the last frame
    float distanceMoved = Vector3.Distance(playerRigidbody.position, lastPosition);

    // Update lastPosition to the current position for the next frame
    lastPosition = playerRigidbody.position;

    // Check if the distance moved is greater than a small threshold (e.g., 0.01f)
    return distanceMoved > 0.01f;

    }

    // Function to play a random footstep sound
    void PlayRandomFootstep()
    {
        // Randomly select a clip from the footstepClips array
        int randomIndex = Random.Range(0, footstepClips.Length);

        // Debug log to ensure the sound is being played
        Debug.Log("Playing footstep sound: " + randomIndex);

        AudioClip randomClip = footstepClips[randomIndex];

        // Play the sound using PlayOneShot or direct assignment
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
