using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SketchbookGuide : MonoBehaviour
{
    InputActions inputActions;
    bool OnSketchbook;
    float duration = 1.0f;
    bool notPlaced;
    [SerializeField] GameObject dragGuide, dropGuide;
    [SerializeField] TextMeshProUGUI inventoryGuide, resultGuide;
    Inventory inventory;

    void Start()
    {
        inventory = Resources.Load<Inventory>("PlayerInventory");
        notPlaced = false;
        OnSketchbook = false;
        inventory.OnContainSticker += ShowGuide;
        inventory.OnEmptyInventory += HideGuide;
        inputActions = FindObjectOfType<InputActionManager>().inputActions;
        inputActions.UI.Trigger.performed += ChangeGuide;
        inputActions.Player.Trigger.performed += ChangeGuide;
    }

    void ShowGuide () {
        notPlaced  = true;
        StartCoroutine(ShowInventoryGuide());
    }

    IEnumerator ShowInventoryGuide () {
        inventoryGuide.text = inventory.items[inventory.items.Count - 1].itemName;
        inventoryGuide.text += " sticker collected";
        yield return new WaitForSeconds(duration);
        inventoryGuide.text = string.Empty;
    }

    void HideGuide () {
        notPlaced = false;
    }

    void ChangeGuide (InputAction.CallbackContext context) {
        OnSketchbook = !OnSketchbook;
        if (OnSketchbook && notPlaced) {
            dragGuide.SetActive(true);
        } else {
            dragGuide.SetActive(false);
            dropGuide.SetActive(false);
            resultGuide.text = string.Empty;
            inventoryGuide.text = string.Empty;
        }
    }

    public void DisplayDropGuide () {
        dragGuide.SetActive(false);
        dropGuide.SetActive(true);
        StopAllCoroutines();
        resultGuide.text = string.Empty;
    }

    public void DisplayResult (bool success) {
        StartCoroutine(DisplayResultForSeconds(success));
    }

    IEnumerator DisplayResultForSeconds (bool success) {
        dropGuide.SetActive(false);
        resultGuide.text = success ? "Sticker placed at the correct location" : "<color=#af001c>Sticker placed at the wrong location</color>";
        yield return new WaitForSeconds(duration);
        resultGuide.text = string.Empty;
        if (notPlaced) {
            dragGuide.SetActive(true);
        }
    }

    void OnDestroy () {
        inventory.OnContainSticker -= ShowGuide;
        inventory.OnEmptyInventory -= HideGuide;
        inputActions.UI.Trigger.performed -= ChangeGuide;
        inputActions.Player.Trigger.performed -= ChangeGuide;
    }
}
