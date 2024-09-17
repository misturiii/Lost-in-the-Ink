using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 100f;
    public float half_color_percent = 75f;
    public float low_color_percent = 50f;
    public Color full_color = Color.gray;
    public Color half_color = Color.green;
    public Color low_color = Color.blue;

    private Renderer targetRenderer;

    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    private Vector3 randomDirection;


    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        targetRenderer.material.color = full_color;
        StartCoroutine(ChangeDirectionRoutine());
    }

    void Update()
{
    if (health <= low_color_percent)
    {
        targetRenderer.material.color = low_color;
    }
    else if (health <= half_color_percent)
    {
        targetRenderer.material.color = half_color;
    }
    else
    {
        targetRenderer.material.color = full_color;
    }
    Debug.Log("Health: " + health);

    // Move the target based on random direction and speed
    transform.Translate(randomDirection * moveSpeed * Time.deltaTime);

    // Keep the target within bounds with a lower height limit
    transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, -10f, 10f),
        Mathf.Clamp(transform.position.y, 0f, 2f),  // Restrict height between 0 and 2 units
        Mathf.Clamp(transform.position.z, -10f, 10f)
    );
}


    
    IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {   
            // Generate a new random direction
            randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            yield return new WaitForSeconds(changeDirectionInterval);
        }
    }


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }

    }

    void Die()
    {
        Destroy(gameObject);
    }
}