using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    public Item item; // Reference to the ScriptableObject that holds item data
    Outline outline;

    void Start () {
        outline = gameObject.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = Vector2.one * 3;
        outline.enabled = false;
    }

    public void Enter () {
        outline.enabled = true;
    }

    public void Exit () {
        outline.enabled = false;
    }
}
