using UnityEngine;

public class TicketBehaviour : ToolBehaviour
{
    GameObject airWall;

    void Awake () {
        airWall = GameObject.FindWithTag("airWall");
    }
    public override void EndBehaviour() {}

    public override void StartBehaviour()
    {
        Destroy(airWall);
        GetComponent<ToolSticker>().ResetSelect(0);
        Destroy(gameObject);
    }
}
