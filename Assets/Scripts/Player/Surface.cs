using UnityEngine;

public class Surface : MonoBehaviour
{
    // The footstep sound array for this surface
    public AudioClip[] footstepClips;

    // Optional: you can add a method to debug or configure other behaviors
    void Start()
    {
        if (footstepClips == null || footstepClips.Length == 0)
        {
            Debug.LogError("No footstep clips assigned for surface: " + gameObject.name);
        }
    }
}
