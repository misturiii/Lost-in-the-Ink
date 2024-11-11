using System.Collections;
using UnityEngine;

public class InteractionManager : MonoBehaviour {
        
    PlayerJump jumpManager;
    DialogueManager dialogueManager;
    PickupObject pickUpManager;
    float waitSec = 0.25f;
    void Start () {
        dialogueManager = GetComponent<DialogueManager>();
        dialogueManager.startDialogue += StartDialogue;
        dialogueManager.endDialogue += EndInteraction;
        
        pickUpManager = GetComponent<PickupObject>();
        pickUpManager.startPick += StartPick;
        pickUpManager.endPick += EndInteraction;

        jumpManager = GetComponent<PlayerJump>();
    }

    void StartDialogue () {
        jumpManager.canJump = false;
        pickUpManager.HidePickupGuide();
        StopAllCoroutines();
    }

    void EndInteraction () {
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown() {
        yield return new WaitForSeconds(waitSec);
        jumpManager.canJump = true;
    }

    void StartPick () {
        jumpManager.canJump = false;
        dialogueManager.HideNpcInteractionGuide();
        StopAllCoroutines();
    }
}