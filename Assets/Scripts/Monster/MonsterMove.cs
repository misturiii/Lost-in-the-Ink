using System.Collections;
using UnityEngine;

public class MonsterMove : MonoBehaviour {
    public float changeDirectionInterval = 2f;
    public float moveSpeed = 2f;
    private Vector3 randomDirection;


    void Start()
    {
        StartCoroutine(ChangeDirectionRoutine());
    }

    void Update()
{
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
}