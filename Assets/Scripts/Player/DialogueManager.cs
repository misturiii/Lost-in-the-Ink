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
    public float rayDistance = 20f;
    private GameObject currentNpc; 
    private bool isDialogueActive = false;

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

    void Update() {
        // Perform a raycast each frame to detect NPCs
        if(isDialogueActive){
            Debug.Log("In Update isdialogueactive is true");
             Debug.Log("Did not detect the RaycastForNPC");

        }
       //是false 进这个
        if(!isDialogueActive){
            Debug.Log("Enter the RaycastForNPC");
            RaycastForNpc();
        }
        
    }

  void AccessDialogue(InputAction.CallbackContext context) {
        Debug.Log("enter AccessDialogue");
        if (dialogue) {
            isDialogueActive = true;
           
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
            isDialogueActive = false;
            Debug.Log("after dialogue, the state of isdialogueactive is " + isDialogueActive);
        }
       

    }

    private void RaycastForNpc() {
        // Cast a ray from the camera's position forward
        Camera mainCamera = Camera.main;
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.blue, 1f);

        if (Physics.Raycast(ray, out hit, rayDistance)) {
            if (hit.collider.CompareTag("Npc")) {
                Debug.Log("NPCCCC HITTTT");
                
                currentNpc = hit.collider.gameObject;  // Store the currently detected NPC
                Debug.Log("NPC detected: " + currentNpc.name); // Debug
                ShowNpcInteractionGuide();

            } else {

                HideNpcInteractionGuide();  // No NPC detected, hide the interaction guide
            }
        } else {
            HideNpcInteractionGuide();  // Nothing hit, hide the interaction guide
        }
    }

    // Show NPC interaction UI prompt
    private void ShowNpcInteractionGuide() {
            dialogue.dialogueObject = currentNpc.GetComponent<NPC>().Enter();
            dialogue.dialogueObject.Reset();

        if (npcInteractionBackground != null && npcInteractionText != null && controllerInteract != null) {
            npcInteractionBackground.enabled = true;
            npcInteractionText.enabled = true;
            controllerInteract.enabled = true;
            npcInteractionText.text = "Press T or Left Click to start talking                    or";
        }
    }

    // Hide NPC interaction UI prompt
    private void HideNpcInteractionGuide() {
        dialogue.gameObject.SetActive(false);
        dialogue.dialogueObject = null;
        if(currentNpc){
            currentNpc.GetComponent<NPC>().Exit();
        }
        if (npcInteractionBackground != null && npcInteractionText != null && controllerInteract != null) {
            npcInteractionBackground.enabled = false;
            npcInteractionText.enabled = false;
            controllerInteract.enabled = false;
        }
    }



}
