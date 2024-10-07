using UnityEngine;
using UnityEngine.Rendering;

public class GlassBehaviour : ToolBehaviour
{
    Camera mainCamera;
    Volume glassFilter;
    Transform target;

    void initialize () {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        glassFilter = mainCamera.GetComponent<Volume>();
        glassFilter.enabled = false;
        mainCamera.cullingMask &= ~(1 << 6);
        target = GameObject.FindWithTag("PlayerTarget").transform;
    }

    public override Transform GetTarget()
    {
        if (!target) {
            initialize();
        }
        return target;
    }

    public override void StartBehaviour()
    {
        mainCamera.cullingMask |= 1 << 6;
        transform.SetParent(target);
        glassFilter.enabled = true;
    }
}
