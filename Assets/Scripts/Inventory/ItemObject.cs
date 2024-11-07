using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item; // Reference to the ScriptableObject that holds item data
    Material mat;
    Color color = FunctionLibrary.LineColor2;
    public int appearCount;

    protected static readonly int lineColorId = Shader.PropertyToID("_LineColor");

    void Start () {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.center = Vector3.zero;
        collider.size = Vector3.one * 10;
        collider.isTrigger = true;
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetColor(lineColorId, Color.white);
        mat.SetFloat("_LineWidth", 0.2f/transform.localScale.x);
    }

    public void Enter () {
        mat.SetColor(lineColorId, color);
    }

    public void Exit () {
        mat.SetColor(lineColorId, Color.white);
    }
}
