using UnityEngine;

public class Location : MonoBehaviour
{
    Transform fountain;
    void Awake()
    {
        fountain = GameObject.FindWithTag("fountain").transform;
    }

    void OnEnable () {
        transform.localPosition = TransformationFunction.WorldToBook(fountain.localPosition);
    }
}