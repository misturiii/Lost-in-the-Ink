using UnityEngine;

public class TicketBehaviour : ToolBehaviour
{
    GameObject airWall;
    Transform target;

    void initialize () {
        airWall = GameObject.FindWithTag("airWall");
        target = GameObject.FindWithTag("TicketTarget").transform;
    }

    public override void StartBehaviour()
    {
        Destroy(airWall);
        Destroy(gameObject);
    }

    public override Transform GetTarget()
    {
        if (!target) {
            initialize();
        }
        return target;
    }
}
