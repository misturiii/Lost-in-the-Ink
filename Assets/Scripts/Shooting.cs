using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera playerCamera;

    public LineRenderer laserLine;
    public float laserDuration = 0.05f;

    public Transform weaponMuzzle; // Add this line

    void Start()
    {
        // If playerCamera is not set in the Inspector, try to find it
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("No camera found. Please assign a camera to the Shooting script.");
                enabled = false; // Disable this script if no camera is found
            }
        }

        // Add these lines
        if (laserLine == null)
        {
            laserLine = gameObject.AddComponent<LineRenderer>();
            laserLine.startWidth = 0.05f; // Reduced width for a thinner laser
            laserLine.endWidth = 0.05f;
            laserLine.material = new Material(Shader.Find("Sprites/Default"));
            laserLine.startColor = Color.blue;
            laserLine.endColor = Color.blue;
        }
        laserLine.enabled = false;

        // Add this block
        if (weaponMuzzle == null)
        {
            weaponMuzzle = transform;
            Debug.LogWarning("Weapon muzzle not set. Using the Shooting script's transform instead.");
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (playerCamera == null) return; // Safety check

        RaycastHit hit;
        Vector3 rayOrigin = weaponMuzzle.position; // Changed from playerCamera.transform.position
        Vector3 rayDirection = playerCamera.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, range)) // IMPORTANT! USE RAYCASTING TO DETECT TARGETS
        {
            Debug.Log("Hit: " + hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            
            // Add these lines
            laserLine.SetPosition(0, rayOrigin);
            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            // Add these lines
            laserLine.SetPosition(0, rayOrigin);
            laserLine.SetPosition(1, rayOrigin + rayDirection * range);
        }

        // Add these lines
        StartCoroutine(ShowLaserForDuration());
    }

    // Add this method
    System.Collections.IEnumerator ShowLaserForDuration()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}
