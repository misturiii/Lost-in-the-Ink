using UnityEngine;

public class LayoutCheck : MonoBehaviour
{
    [SerializeField] private Transform colliders;
    [SerializeField] private GameObject win;
    private int count;
    
    public void Check()
    {
        count++;
        
        // 打印当前 count 的值
        Debug.Log("Current count: " + count);
        
        // 打印 colliders 和 transform 的子对象数量
        Debug.Log("Colliders child count: " + colliders.childCount);
        Debug.Log("Transform child count: " + transform.childCount);
        
        // 检查胜利条件
        if (colliders.childCount == 0 && count == transform.childCount)
        {
            win.SetActive(true);
            Debug.Log("YOU WIN");
        }
        else
        {
            Debug.Log("Conditions not met yet.");
        }
    }
}
