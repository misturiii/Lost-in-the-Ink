using UnityEngine;

public abstract class ToolBehaviour : MonoBehaviour
{
    abstract public void StartBehaviour();

    abstract public Transform GetTarget();
}
