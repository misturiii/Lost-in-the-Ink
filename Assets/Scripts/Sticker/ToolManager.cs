using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum Tool {Camera, RubikCube, TrashCan};
delegate bool Behaviour (float input);
public class ToolManager : MonoBehaviour
{
    [SerializeField] Sprite[] icons;
    [SerializeField] InventoryDisplay inventoryDisplay;
    [SerializeField] Image iconDisplay;
    Action<InputAction.CallbackContext>[] actions;
    InputAction[] keys;
    float duration = 0.5f, progress = 0;
    float outlineWWidth = 6;
    Tool currentTool;
    SketchbookGuide sketchbookGuide;

    void Start () {
        InputActions inputActions = FindObjectOfType<InputActionManager>().inputActions;
        sketchbookGuide = GameObject.Find("PlayerGuide").GetComponentInChildren<SketchbookGuide>();
        actions = new Action<InputAction.CallbackContext>[] {Camera, RubikCube, TrashCan};
        keys = new InputAction[] {inputActions.UI.Duplicate, inputActions.UI.Rotate, inputActions.UI.Delete};
        iconDisplay.material.SetFloat("_LineWidth", outlineWWidth);
    }

    public void AddTool (Tool tool) {
        keys[(int)tool].performed += actions[(int)tool];
        keys[(int)tool].canceled += EndTool;
    }

    IEnumerator Press (Tool tool, ItemSticker sticker, Behaviour behaviour, float input) {
        if (currentTool != tool) {
            progress = 0;
            currentTool = tool;
        }
        if (progress == 0) {
            iconDisplay.enabled = true;
            iconDisplay.sprite = icons[(int)tool];
        } 
        while (keys[(int)tool].inProgress) {
            if (currentTool != tool) {
                EndTool(new InputAction.CallbackContext());
            }
            progress += Time.deltaTime;
            if (!sticker) {
                EndTool(new InputAction.CallbackContext());
            } else {
                iconDisplay.transform.position = sticker.GetCenterPosition();
            }
            if (progress >= duration) {
                behaviour(input);
                progress -= duration;
            }
            yield return null;
        }
        iconDisplay.enabled = false;
    }

    void Camera (InputAction.CallbackContext context) {
        if (inventoryDisplay.currentSelected is ItemSticker) {
            sketchbookGuide.toolGuide.text = "Hold to duplicate the sticker";
            ItemSticker sticker = (ItemSticker) inventoryDisplay.currentSelected;
            StartCoroutine(Press(Tool.Camera, sticker, sticker.Camera, 0));
        } else {
            sketchbookGuide.DisplayResult("Cannot apply to inventory");
        }
    }

    void TrashCan (InputAction.CallbackContext context) {
        if (inventoryDisplay.currentSelected is ItemSticker) {
            sketchbookGuide.toolGuide.text = "Hold to delete the sticker";
            ItemSticker sticker = (ItemSticker) inventoryDisplay.currentSelected;
            StartCoroutine(Press(Tool.TrashCan, sticker, sticker.TrashCan, 0));
        } else {
            sketchbookGuide.DisplayResult("Cannot apply to inventory");
        }
    }

    void RubikCube (InputAction.CallbackContext context) {
        if (inventoryDisplay.currentSelected is ItemSticker) {
            sketchbookGuide.toolGuide.text = "Hold to rotate the sticker";
            ItemSticker sticker = (ItemSticker) inventoryDisplay.currentSelected;
            StartCoroutine(Press(Tool.RubikCube, sticker, sticker.Mirror, context.ReadValue<float>()));
        } else {
            sketchbookGuide.DisplayResult("Cannot apply to inventory");
        }
    }

    void EndTool (InputAction.CallbackContext context) {
        StopAllCoroutines();
        progress = 0;
        iconDisplay.enabled = false;
        sketchbookGuide.toolGuide.text = "";
    }
}
