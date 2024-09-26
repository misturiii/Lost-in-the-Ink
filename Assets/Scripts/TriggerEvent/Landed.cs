using UnityEngine;

public class Landed : TriggerEvent
{
    bool landed = false;
    public override bool Finished()
    {
        return landed;
    }

    void OnCollisionEnter (Collision collision) {
        if (collision.collider.tag == "Player") {
            landed = true;
        }
    }
}
