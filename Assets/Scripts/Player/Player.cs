using UnityEngine;

public class Player : MonoBehaviour
{
    Move move;
    Rotate rotate;

    void Start () {
        move = GetComponent<Move>();
        rotate = GetComponentInChildren<Rotate>();
    }
    
    public void EnableMoveAndRotate (bool enable) {
        move.enabled = enable;
        rotate.enabled = enable;
    }
}
