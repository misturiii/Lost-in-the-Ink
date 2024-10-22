using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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
    public float rayDistance = 1.5f;
    private GameObject currentNpc; 
    private bool isDialogueActive = false;
    public Transform skipBar;
    public GameObject skipText;
    public GameObject skip;
    float skipProgress;
    float skipDuration = 2.0f;
    bool cooling;

    void Start () {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.Click.Enable();
        inputActions.Player.Click.performed += AccessDialogue;
        inputActions.Player.Click.canceled += PauseSkip;

        // 初始化UI提示为隐藏状态
        if (npcInteractionBackground != null && npcInteractionText != null && controllerInteract != null) {
            npcInteractionBackground.enabled = false;
            npcInteractionText.enabled = false;
            controllerInteract.enabled = false;
        }

        pickupObject = FindObjectOfType<PickupObject>();
        fountainSticker = GameObject.Find("FountainSticker");
        player = GameObject.FindWithTag("Player");
        
        cooling = false;

        // 禁用 Fountain Sticker 的 tag
        if (fountainSticker != null) {
            fountainSticker.tag = "Untagged";  // 暂时移除 PickableItem tag
        }
    }

    IEnumerator CoolDown () {
        cooling = true;
        yield return new WaitForSeconds(1.0f);
        cooling = false;
    }

    void Update() {
    
        if(!isDialogueActive){
            // Debug.Log("Enter the RaycastForNPC");
            RaycastForNpc();
        }
        if (skip.activeSelf && inputActions.Player.Click.inProgress) {
            if (skipProgress > 0.1) {
                skipBar.parent.gameObject.SetActive(true);
                skipText.SetActive(false);
            }
            skipProgress += Time.deltaTime / skipDuration;
            skipBar.localScale = new Vector3 (skipProgress, 1, 0);
        }
        if (skipProgress >= 1) {
            dialogue.Reset();
            OnDialogueEnd();
        }
    }

  void AccessDialogue(InputAction.CallbackContext context) {
        // Debug.Log("enter AccessDialogue");
        if (dialogue && currentNpc && !cooling) {
            isDialogueActive = true;
            // inputActions.Player.Disable();
            inputActions.Player.Move.Disable();
            inputActions.Player.Look.Disable(); 
            inputActions.Player.Trigger.Disable();
           
            if (!dialogue.DisplayDialogue()) {
                StartCoroutine(CoolDown());
            }

            // 隐藏 NPC 交互提示
            if (npcInteractionBackground != null && npcInteractionText != null&& controllerInteract != null) {
                npcInteractionBackground.enabled = false;
                npcInteractionText.enabled = false;
                controllerInteract.enabled = false;
            }
        }
    }

    void PauseSkip (InputAction.CallbackContext context) {
        skipBar.parent.gameObject.SetActive(false);
        skipText.SetActive(true);
        skipProgress = 0;
        skipBar.localScale = new Vector3(0, 1, 0);
    }


    // 对话结束时调用
    public void OnDialogueEnd() {
        // 当对话结束时，启用 Fountain Sticker 的 PickableItem tag
        skip.SetActive(true);
        if (fountainSticker != null) {
            fountainSticker.tag = "PickableItem";
           
            Debug.Log("after dialogue, the state of isdialogueactive is " + isDialogueActive);
        }
        inputActions.Player.Move.Enable();
        inputActions.Player.Look.Enable(); 
        inputActions.Player.Trigger.Enable();
        isDialogueActive = false;
    }

    private void RaycastForNpc() {
        // Cast a ray from the camera's position forward
        Camera mainCamera = Camera.main;
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.blue, 1f);

        if (Physics.Raycast(ray, out hit, rayDistance)) {
            if (hit.collider.CompareTag("Npc")) {
                
                currentNpc = hit.collider.gameObject;  // Store the currently detected NPC
                // Debug.Log("NPC detected: " + currentNpc.name); // Debug
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
            skip.SetActive(dialogue.notFirstTime);


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
            currentNpc = null; 
        }
        if (npcInteractionBackground != null && npcInteractionText != null && controllerInteract != null) {
            npcInteractionBackground.enabled = false;
            npcInteractionText.enabled = false;
            controllerInteract.enabled = false;
        }
    }



}
