using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CheckManager : MonoBehaviour
{
    public static CheckManager Instance;
    [SerializeField] private List<Item> items = new List<Item>();

    public GameObject winText;
    public GameObject sketchbookUI;
    private bool winConditionMet = false;
    InputActionManager inputActionManager;
    public GameObject initButton;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        inputActionManager = FindObjectOfType<InputActionManager>();
    }


    public void RegisterItem(Item item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
        }
    }


    public void CheckWinCondition()
    {
        foreach (var item in items)
        {
            if (!item.AreAllChecksTrue())
            {
                Debug.Log(item.itemName + " fail the check");
                return;
            }
        }
        winConditionMet = true;
        if (!sketchbookUI.activeSelf){
            ShowWinText();
        }

    }

    public void OnSketchbookClosed(){
        if (winConditionMet && !sketchbookUI.activeSelf){
            ShowWinText();
            EventSystem.current.SetSelectedGameObject(initButton);
        }
    }
    private void ShowWinText()
    {

        if (winText != null)
        {
            winText.SetActive(true);
            inputActionManager.SetAllActive(false);
            Debug.Log("YOU WIN!");
        }
        else
        {
            Debug.LogWarning("Win text is not assigned in CheckManager.");
        }
    }

    public void BackToMenu()
    {
        Resources.Load<Inventory>("PlayerInventory").Clear();
        SceneManager.LoadScene("StartPage");
    }


    public void ContinueExplore()
    {
        if (winText != null)
        {
            Destroy(winText);
            winText = null;
            inputActionManager.SetPlayerActive(true);
        }
    }
}