using UnityEngine;
using UnityEngine.UI;

public class StickerChange : MonoBehaviour {
    RawImage image;
    [SerializeField]Sprite coloredImage;

    void Start () {
        image = GetComponent<RawImage>();
    }

    public void Change () {
        image.texture = coloredImage.texture;
    }
}