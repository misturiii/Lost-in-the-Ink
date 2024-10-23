using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    // Reference to the slider component
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial volume from the slider's value (between 0 and 1)
        AudioListener.volume = volumeSlider.value;

        // Add a listener to detect changes to the slider value
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // This method will be called whenever the slider's value changes
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // Adjusts the volume of the entire game
    }
}
