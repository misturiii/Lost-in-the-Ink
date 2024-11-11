using UnityEngine;

public class StickerLocation : MonoBehaviour
{
    [SerializeField] Transform obj;

    void OnEnable () {
        transform.localPosition = FunctionLibrary.WorldToBook(obj.position);
    }
}
