using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    InputActions inputActions;

    // 新增的UI元素：提示“Press T or left click to start talking”
    public GameObject interactionGuide;
    float rayDistance = 3f;
    private GameObject currentNpc; 
    private bool isDialogueActive = false;
    public Transform skipBar;
    public GameObject skipText;
    public GameObject skip;
    float skipProgress;
    float skipDuration = 1.0f;
    bool cooling;

    public delegate void DialoggueBehaviour();
    public DialoggueBehaviour startDialogue;
    public DialoggueBehaviour endDialogue;

    NPC npc = null;

    void Start () {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.Click.Enable();
        inputActions.Player.Click.performed += AccessDialogue;
        inputActions.Player.Click.canceled += PauseSkip;

        interactionGuide.SetActive(false);
        
        cooling = false;
    }

    IEnumerator CoolDown () {
        cooling = true;
        yield return new WaitForSeconds(1.0f);
        cooling = false;
    }

    void Update() {
    
        if(!isDialogueActive){
            // Debug.Log("Enter the RaycastForNPC");
            RaycastFromCamera();
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
            inputActions.Player.Jump.Disable();
           
            if (!dialogue.DisplayDialogue()) {
                StartCoroutine(CoolDown());
            }

            // 隐藏 NPC 交互提示
            interactionGuide.SetActive(false);
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
        npc.DialogueEnds();

        inputActions.Player.Move.Enable();
        inputActions.Player.Look.Enable(); 
        inputActions.Player.Trigger.Enable();
        inputActions.Player.Jump.Enable();
        isDialogueActive = false;
        endDialogue?.Invoke();
    }

    private void RaycastFromCamera() {
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
        npc = currentNpc.GetComponent<NPC>();
        dialogue.dialogueObject = npc.Enter();
        dialogue.dialogueObject.Reset();
        skip.SetActive(dialogue.notFirstTime);


        interactionGuide.SetActive(true);
        startDialogue?.Invoke();
    }

    // Hide NPC interaction UI prompt
    public void HideNpcInteractionGuide() {
        npc = null;
        dialogue.gameObject.SetActive(false);
        dialogue.dialogueObject = null;
        if(currentNpc){
            currentNpc.GetComponent<NPC>().Exit();
            currentNpc = null; 
        }
        interactionGuide.SetActive(false);
    }
}
