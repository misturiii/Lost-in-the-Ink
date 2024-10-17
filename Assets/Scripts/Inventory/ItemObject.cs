using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item; // Reference to the ScriptableObject that holds item data
    QuickOutline outline;
    Color outlineColor = Color.black;

    void Start () {
        outline = gameObject.AddComponent<QuickOutline>();
        outline.OutlineColor = outlineColor;
        outline.OutlineMode = QuickOutline.Mode.OutlineVisible;
        outline.OutlineWidth = 10;
        outline.enabled = false;
    }

    public void Enter () {
        outline.enabled = true;
    }

    public void Exit () {
        outline.enabled = false;
    }
}
