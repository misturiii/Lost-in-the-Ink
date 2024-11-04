
using UnityEngine;

public class LayoutCheck : MonoBehaviour
{
    [SerializeField] Transform colliders;
    [SerializeField] GameObject win;
    int count;
    
    public void Check () {
        count++;
        if (colliders.childCount == 0 && count == transform.childCount) {
            win.SetActive(true);
            Debug.Log("YOU WIN");
        }
    }
}
