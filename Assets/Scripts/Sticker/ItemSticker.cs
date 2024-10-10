using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSticker : Sticker
{
    protected Transform copy = null;
    [SerializeField] protected GameObject prefab;
    [SerializeField] public string stickername;
    public TextMeshProUGUI feedbackText;  
    private readonly float xTolerance = 160f; 
    private readonly float yTolerance = 160f; 
    private Vector3 initialPosition;  

    // 存储每个 Sticker 的目标位置
    private readonly Dictionary<string, List<Vector3>> stickerTargetPositions = new Dictionary<string, List<Vector3>>()
    {
        { "Bench(Clone)", new List<Vector3> { new Vector3(150.00f, 330f, 0.00f) } },
        { "IceCream(Clone)", new List<Vector3> { new Vector3(370f, 80.0f, 0.00f) } }
    };

        // 追踪每个 Sticker 是否被正确放置
    private readonly Dictionary<string, bool> isPlacedCorrectly = new Dictionary<string, bool>();

    protected override void Initialize() {
        initialPosition = transform.position;

        // 初始化 isPlacedCorrectly 字典
        foreach (var stickerName in stickerTargetPositions.Keys)
        {
            if (!isPlacedCorrectly.ContainsKey(stickerName))
            {
                isPlacedCorrectly.Add(stickerName, false);
            }
        }
        initialPosition = transform.position;

        
        feedbackText = GameObject.Find("/Canvas/FeedbackText")?.GetComponent<TextMeshProUGUI>();

        if (feedbackText != null)
        {
            feedbackText.enabled = true;
            feedbackText.text = "";  
            Debug.Log("FeedbackText initialized successfully.");
        }
        else
        {
            Debug.LogError("FeedbackText not found or assigned! Please check the UI setup.");
        }

        Debug.Log($"Sticker Initialized: Name = {name}, Index = {index}, Position = {transform.position}");
        }   

    protected override void UseStickerBefore() {}
    protected override void UseStickerAfter() {
        Vector3 droppedPosition = transform.position;
        Debug.Log($"Sticker Dropped: Name = {name}, Index = {index}, Position = {droppedPosition}");

        // 检查 Sticker 是否有指定的目标位置
        if (stickerTargetPositions.ContainsKey(name))
        {
            bool isInTargetArea = CheckIfInTargetArea(droppedPosition, stickerTargetPositions[name]);

            if (isInTargetArea)
            {
                // ShowFeedbackMessage("Success", 1f);
                isPlacedCorrectly[name] = true; // 设置为已放置正确
                Debug.Log("Success: Sticker placed in the correct area!");
                if (!copy) {
                    copy = Instantiate(prefab).transform;
                }
                copy.localPosition = TransformationFunction.BookToWorld(transform.localPosition);
                copy.localEulerAngles = Vector3.zero;
                return;
            }
            else
            {
                // ShowFeedbackMessage("Try Again", 1f);
                Debug.Log("Try Again: Sticker not placed in the correct area.");
                inventoryDisplay.AddToInventory(this, null);
                if (copy) {
                    Destroy(copy.gameObject);
                }
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Debug.Log($"Sticker {name} does not have a specified target area.");
        }
        
    }

    private bool CheckIfInTargetArea(Vector3 droppedPosition, List<Vector3> targetPositions)
    {
        foreach (var targetPosition in targetPositions)
        {
            bool isXInRange = Mathf.Abs(droppedPosition.x - targetPosition.x) <= xTolerance;
            bool isYInRange = Mathf.Abs(droppedPosition.y - targetPosition.y) <= yTolerance;

            if (isXInRange && isYInRange)
            {
                return true; // 只要有一个目标区域满足条件，则返回 true
            }
        }

        return false; // 如果所有目标区域都不满足，则返回 false
    }

    private void ReturnToInventory() {
        transform.position = initialPosition;
        Debug.Log($"Sticker returned to initial position: {initialPosition}");
    }

    private void ShowFeedbackMessage(string message, float duration) {
        if (feedbackText != null) {
            feedbackText.text = message;
            feedbackText.enabled = true;
            Debug.Log($"[ShowFeedbackMessage] Displaying message: {message}");
            StartCoroutine(HideFeedbackMessage(duration));
        }
        else
        {
            Debug.LogError("[ShowFeedbackMessage] feedbackText is not assigned or found.");
        }
    }

    private IEnumerator HideFeedbackMessage(float duration) {
        yield return new WaitForSeconds(duration);
        if (feedbackText != null)
        {
            feedbackText.text = "";
            feedbackText.enabled = false;
            Debug.Log("[HideFeedbackMessage] Feedback message hidden successfully.");
        }
        else
        {
            Debug.LogError("[HideFeedbackMessage] feedbackText is not assigned or found.");
        }
    }
}
