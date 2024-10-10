using UnityEngine;

public class PlayrLocation : MonoBehaviour
{
    Transform player;
    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void OnEnable () {
        transform.localPosition = TransformationFunction.WorldToBook(player.localPosition);
    }
}
