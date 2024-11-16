using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public Transform player; // The player's transform
    public RectTransform arrow; // The RectTransform of the arrow image on the 2D canvas
    public Camera playerCamera; // The camera used to view the 3D world

    // The initial offset for the arrow's initial rotation
    public float initialOffset = 270f;

    void Awake()
    {
        // Find the player transform using a tag (ensure the player tag is assigned in the Inspector)
        player = GameObject.FindWithTag("Player").transform;
    }

    void OnEnable()
    {
        // Ensure that on enabling, we update the position (this will be based on your existing function)
        transform.localPosition = FunctionLibrary.WorldToBook(player.localPosition);

        // Update the arrow rotation to ensure it's aligned when the object is enabled
        UpdateArrowRotation();
    }

    void UpdateArrowRotation()
    {
        // Get the camera's current Y-axis rotation (the yaw)
        float cameraYaw = playerCamera.transform.eulerAngles.y;

        // Apply the opposite of the camera's yaw to invert the rotation direction
        // Also apply any initial offset to correct the default arrow facing
        arrow.rotation = Quaternion.Euler(0, 0, -cameraYaw + initialOffset);
    }

    // Update the arrow every frame to ensure it stays aligned with the player's camera rotation
    void Update()
    {
        UpdateArrowRotation(); // Refresh the arrow's rotation each frame
    }
}
