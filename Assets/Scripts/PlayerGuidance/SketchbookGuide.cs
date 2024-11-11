using System.Collections;
using TMPro;
using UnityEngine;

public class SketchbookGuide : MonoBehaviour
{
    float duration = 1.0f;
    [SerializeField] GameObject dragGuide, dropGuide;
    [SerializeField] TextMeshProUGUI inventoryGuide, resultGuide;
    GameObject current = null;
    Inventory inventory;
    public TextMeshProUGUI toolGuide;

    void Start()
    {
        inventory = Resources.Load<Inventory>("PlayerInventory");
        inventory.OnContainSticker += ShowGuide;
        inventory.OnEmptyInventory += HideGuide;
    }

    void ShowGuide () {
        DisplayDragGuide();
        StartCoroutine(ShowInventoryGuide());
    }

    IEnumerator ShowInventoryGuide () {
        inventoryGuide.text = inventory.newItem;
        inventoryGuide.text += " sticker collected";
        yield return new WaitForSeconds(duration);
        inventoryGuide.text = string.Empty;
    }

    void HideGuide () {
        current?.SetActive(false);
    }

    public void DisplayDropGuide () => DisplayGuide(dropGuide);

    public void DisplayDragGuide () => DisplayGuide(dragGuide);

    void DisplayGuide (GameObject guide) {
        current?.SetActive(false);
        current = guide;
        resultGuide.text = string.Empty;
        current.SetActive(true);
    }

    public void DisplayResult (string result) {
        StartCoroutine(DisplayResultForSeconds(result));
    }

    IEnumerator DisplayResultForSeconds (string result) {
        current?.SetActive(false);
        resultGuide.text = result;
        yield return new WaitForSeconds(duration);
        resultGuide.text = string.Empty; 
        current?.SetActive(true);
    }

    void OnDestroy () {
        inventory.OnContainSticker -= ShowGuide;
        inventory.OnEmptyInventory -= HideGuide;
    }
}
