using System.Collections;
using UnityEngine;

public class InteractionManager : MonoBehaviour {
        
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
    }

    void StartDialogue () {
        pickUpManager.HidePickupGuide();
        StopAllCoroutines();
    }

    void EndInteraction () {
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown() {
        yield return new WaitForSeconds(waitSec);
    }

    void StartPick () {
        dialogueManager.HideNpcInteractionGuide();
        StopAllCoroutines();
    }
}