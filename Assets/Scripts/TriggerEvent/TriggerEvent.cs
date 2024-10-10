using UnityEngine;

public abstract class TriggerEvent : MonoBehaviour {
    [SerializeField] protected float distance = 1;
    public abstract bool Finished();
    protected Transform player;
    protected Vector3 curPosition;

    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        curPosition = transform.localPosition;
        curPosition.y = 0;
    }

    protected bool FindPlayer () {
        Vector3 playerPosition = player.localPosition;
        playerPosition.y = 0;
        return (playerPosition - curPosition).magnitude <= distance;
    }

}