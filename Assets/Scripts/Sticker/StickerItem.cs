using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.EventSystems;

public class StickerItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // 改变透明度表示正在拖动
        canvasGroup.blocksRaycasts = false; // 允许拖动时穿透检测
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta; // 随鼠标移动贴纸
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; // 恢复透明度
        canvasGroup.blocksRaycasts = true; // 停止拖动时阻止穿透
    }
}
