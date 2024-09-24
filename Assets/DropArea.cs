using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.EventSystems;
using TMPro; // 引入 TextMeshPro 命名空间

public class DropArea : MonoBehaviour, IDropHandler
{
    public string requiredStickerTag; // 匹配的贴纸标签
    public TextMeshProUGUI errorMessage; // 错误信息的 UI 组件
    public float errorDisplayDuration = 2f; // 错误提示显示时间

    private void Start()
    {
        // 在游戏开始时隐藏错误信息
        errorMessage.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedObject = eventData.pointerDrag;

        if (draggedObject != null)
        {
            // 调试信息：输出拖动对象的标签和需要的标签
            Debug.Log("Dragged Object Tag: " + draggedObject.tag);
            Debug.Log("Required Sticker Tag: " + requiredStickerTag);

            // 检查是否匹配贴纸标签
            if (draggedObject.CompareTag(requiredStickerTag))
            {
                // 匹配成功，放置贴纸
                draggedObject.transform.SetParent(transform);
                draggedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                Debug.Log("贴纸放置成功！");
            }
            else
            {
                // 不匹配，显示错误信息
                Debug.Log("贴纸不匹配！");
                StartCoroutine(ShowErrorMessage("贴纸不匹配！"));
            }
        }
    }

    // 协程：显示错误信息一段时间后隐藏
    private IEnumerator ShowErrorMessage(string message)
    {
        errorMessage.text = message; // 设置错误信息文本
        errorMessage.gameObject.SetActive(true); // 启用错误信息 UI
        yield return new WaitForSeconds(errorDisplayDuration); // 等待指定的时间
        errorMessage.gameObject.SetActive(false); // 隐藏错误信息
    }
}
