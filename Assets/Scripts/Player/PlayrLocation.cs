using UnityEngine;

public class PlayrLocation : MonoBehaviour
{
    [SerializeField, Min(0)] float sketchbookWidth = 40, sketchbookHeight = 30;  
    [SerializeField] float inventoryWidth = 400;

    Transform player;
    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void OnEnable () {
        float x = -(player.localPosition.x / sketchbookWidth) * (Screen.width - inventoryWidth);
        float y = -(player.localPosition.z / sketchbookHeight) * Screen.height;
        transform.position = new Vector2(x, y);
    }
}
