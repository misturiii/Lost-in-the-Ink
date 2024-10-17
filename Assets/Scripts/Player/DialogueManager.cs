using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    InputActions inputActions;
    private GameObject fountainSticker;  // 追踪 Fountain Sticker
    private PickupObject pickupObject;
    private GameObject player;  // 追踪 Player 位置

    // 新增的UI元素：提示“Press T or left click to start talking”
    public Image npcInteractionBackground;  // 背景图片
    public TextMeshProUGUI npcInteractionText;  // 提示文字

    public Image controllerInteract;
    void Start () {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.Click.Enable();
        inputActions.Player.Click.performed += AccessDialogue;

        // 初始化UI提示为隐藏状态
        if (npcInteractionBackground != null && npcInteractionText != null && controllerInteract != null) {
            npcInteractionBackground.enabled = false;
            npcInteractionText.enabled = false;
            controllerInteract.enabled = false;
        }

        pickupObject = FindObjectOfType<PickupObject>();
        fountainSticker = GameObject.Find("FountainSticker");
        player = GameObject.FindWithTag("Player");

        // 禁用 Fountain Sticker 的 tag
        if (fountainSticker != null) {
            fountainSticker.tag = "Untagged";  // 暂时移除 PickableItem tag
        }
    }

    void AccessDialogue(InputAction.CallbackContext context) {
        if (dialogue) {
            dialogue.DsiplayDialogue();

            // 隐藏 NPC 交互提示
            if (npcInteractionBackground != null && npcInteractionText != null&& controllerInteract != null) {
                npcInteractionBackground.enabled = false;
                npcInteractionText.enabled = false;
                controllerInteract.enabled = false;
            }
        }
    }

    // 对话结束时调用
    public void OnDialogueEnd() {
        // 当对话结束时，启用 Fountain Sticker 的 PickableItem tag
        if (fountainSticker != null) {
            fountainSticker.tag = "PickableItem";
            Debug.Log("Fountain Sticker is now pickable.");

            // 检查玩家是否在 Fountain Sticker 的范围内
            CheckPlayerProximityToSticker();
        }
    }

    // 检查玩家和 Fountain Sticker 之间的距离
    private void CheckPlayerProximityToSticker() {
        if (player != null && fountainSticker != null) {
            float distance = Vector3.Distance(player.transform.position, fountainSticker.transform.position);
            
            // 如果玩家距离 Fountain Sticker 在一定范围内，则手动触发 pickup 提示
            if (distance < 3.0f) {  // 距离值可以根据你的游戏设计调整
                Debug.Log("Player is close to the Fountain Sticker after dialogue.");
                
                // 模拟 OnTriggerEnter 的行为
                SimulateTriggerEnter();
            } else {
                Debug.Log("Player is too far from the Fountain Sticker.");
            }
        }
    }

    // 手动模拟触发 OnTriggerEnter 行为
    private void SimulateTriggerEnter() {
        Collider fountainCollider = fountainSticker.GetComponent<Collider>();
        if (fountainCollider != null) {
            Debug.Log("Simulating OnTriggerEnter for the Fountain Sticker.");
            pickupObject.OnTriggerEnter(fountainCollider);  // 模拟玩家重新进入检测区域
        } else {
            Debug.LogError("Fountain Sticker does not have a Collider.");
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Npc") {
            dialogue.dialogueObject = collider.GetComponent<NPC>().Enter();
            dialogue.dialogueObject.Reset();

            // 显示NPC交互提示
            if (npcInteractionBackground != null && npcInteractionText != null && controllerInteract != null) {
                npcInteractionBackground.enabled = true;
                npcInteractionText.enabled = true;
                controllerInteract.enabled =true;
                npcInteractionText.text = "Press T or Left Click to start talking                    or";
            }
        } 
        if (collider.tag == "PickableItem") {
            collider.GetComponent<ItemObject>().Enter();
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "Npc") {
            dialogue.gameObject.SetActive(false);
            dialogue.dialogueObject = null;
            collider.GetComponent<NPC>().Exit();
            if (npcInteractionBackground != null && npcInteractionText != null&& controllerInteract != null) {
                npcInteractionBackground.enabled = false;
                npcInteractionText.enabled = false;
                controllerInteract.enabled = false;
            }
        }
        if (collider.tag == "PickableItem") {
            collider.GetComponent<ItemObject>().Exit();
        }
    }
}
