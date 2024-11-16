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
    float duration = 0.75f, progress = 0;
    float outlineWWidth = 6;
    Tool currentTool;
    SketchbookGuide sketchbookGuide;
    [SerializeField]Transform toolControl;

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
        toolControl.gameObject.SetActive(true);
        toolControl.GetChild((int)tool).gameObject.SetActive(true);
    }

    IEnumerator Press (Tool tool, ItemSticker sticker, Behaviour behaviour, float input) {
        if (currentTool != tool) {
            progress = 0;
            currentTool = tool;
        }
        if (progress == 0) {
            iconDisplay.enabled = true;
            iconDisplay.sprite = icons[(int)tool];
            iconDisplay.transform.localScale = new Vector3(-input, 1, 1);
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
                if (!behaviour(input)) {
                    progress = 0;
                    break;
                }
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
            StartCoroutine(Press(Tool.Camera, sticker, sticker.Camera, -1));
        } else {
            DisplayError(inventoryDisplay.currentSelected);
        }
    }

    void TrashCan (InputAction.CallbackContext context) {
        if (inventoryDisplay.currentSelected is ItemSticker) {
            sketchbookGuide.toolGuide.text = "Hold to delete the sticker";
            ItemSticker sticker = (ItemSticker) inventoryDisplay.currentSelected;
            StartCoroutine(Press(Tool.TrashCan, sticker, sticker.TrashCan, -1));
        } else {
            DisplayError(inventoryDisplay.currentSelected);
        }
    }

    void RubikCube (InputAction.CallbackContext context) {
        if (inventoryDisplay.currentSelected is ItemSticker) {
            sketchbookGuide.toolGuide.text = "Hold to rotate the sticker";
            ItemSticker sticker = (ItemSticker) inventoryDisplay.currentSelected;
            StartCoroutine(Press(Tool.RubikCube, sticker, sticker.Mirror, context.ReadValue<float>()));
        } else {
            DisplayError(inventoryDisplay.currentSelected);
        }
    }

    void EndTool (InputAction.CallbackContext context) {
        StopAllCoroutines();
        progress = 0;
        iconDisplay.enabled = false;
        sketchbookGuide.toolGuide.text = "";
    }

    void DisplayError (Selectable selectable) {
        if (selectable is InventoryBox) {
            sketchbookGuide.DisplayResult("Cannot apply to inventory");
        } else if (selectable is PieceSticker) {
            sketchbookGuide.DisplayResult("Cannot apply to incomplete sticker");
        }
    }
}
