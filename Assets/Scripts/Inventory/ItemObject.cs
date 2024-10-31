using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item item; // Reference to the ScriptableObject that holds item data
    Material mat;

    protected static readonly int useOutlineId = Shader.PropertyToID("_UseOutline");

    void Start () {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.center = Vector3.zero;
        collider.size = Vector3.one * 10;
        collider.isTrigger = true;
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetInt(useOutlineId, 0);
    }

    public void Enter () {
        mat.SetInt(useOutlineId, 1);
    }

    public void Exit () {
        mat.SetInt(useOutlineId, 0);
    }
}
