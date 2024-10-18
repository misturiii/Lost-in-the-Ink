using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SketchbookGuide : MonoBehaviour
{
    string sketchbookGuide = "Open the sketchbook to place the sticker";
    string dragGuide = "Use mouse or hold left trigger and move left stick to drag";
    string dropGuide = "Release the mouse or left trigger to drop";
    TextMeshProUGUI textComponent;
    InputActions inputActions;
    bool OnSketchbook;
    float duration = 1.0f;
    bool notPlaced;

    void Start()
    {
        Inventory inventory = Resources.Load<Inventory>("PlayerInventory");
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.text = sketchbookGuide;
        notPlaced = false;
        OnSketchbook = false;
        inventory.OnContainSticker += ShowGuide;
        inventory.OnEmptyInventory += HideGuide;
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Trigger.performed += ChangeGuide;
        inputActions.Player.Trigger.performed += ChangeGuide;
    }

    void ShowGuide () {
        textComponent.enabled = notPlaced = true;
    }

    void HideGuide () {
        notPlaced = false;
    }

    void ChangeGuide (InputAction.CallbackContext context) {
        OnSketchbook = !OnSketchbook;
        textComponent.text = DecideGuide();
    }

    string DecideGuide () {
        if (OnSketchbook) {
            textComponent.color = Color.black;
            return dragGuide;
        } else {
            textComponent.color = Color.white;
            return sketchbookGuide;
        }
    } 

    public void DisplayDropGuide () {
        textComponent.text = dropGuide;
        StopAllCoroutines();
    }

    public void DisplayResult (bool success) {
        StartCoroutine(DisplayResultForSeconds(success));
    }

    IEnumerator DisplayResultForSeconds (bool success) {
        textComponent.text = success ? "Sticker placed at the correct location" : "<color=#af001c>Sticker placed at the wrong location</color>";
        yield return new WaitForSeconds(duration);
        textComponent.text = dragGuide;
        textComponent.enabled = notPlaced;
    }

}
