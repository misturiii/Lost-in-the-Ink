using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    InputActions inputActions;

    void Start () {
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.Player.Click.Enable();
        inputActions.Player.Click.performed += AccessDialogue;
    }

    void AccessDialogue (InputAction.CallbackContext context) {
        if (dialogue) {
            dialogue.DsiplayDialogue();
        }
    }

    void OnTriggerEnter (Collider collider) {
        if (collider.tag == "Npc") {
            dialogue.dialogueObject = collider.GetComponent<NPC>().Enter();
            dialogue.dialogueObject.Reset();
        } 
        if (collider.tag == "PickableItem") {
            collider.GetComponent<ItemObject>().Enter();
        }
    }
    void OnTriggerExit (Collider collider) {
        if (collider.tag == "Npc") {
            dialogue.gameObject.SetActive(false);
            dialogue.dialogueObject = null;
            collider.GetComponent<NPC>().Exit();
        }
        if (collider.tag == "PickableItem") {
            collider.GetComponent<ItemObject>().Exit();
        }
    }
}