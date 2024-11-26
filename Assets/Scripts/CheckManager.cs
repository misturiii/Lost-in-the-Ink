// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.SceneManagement;

// public class CheckManager : MonoBehaviour
// {
//     public static CheckManager Instance;
//     [SerializeField] private List<Item> items = new List<Item>();

//     public GameObject winText;
//     public GameObject sketchbookUI;
//     private bool winConditionMet = false;
//     InputActionManager inputActionManager;
//     public GameObject initButton;
//     void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//         inputActionManager = FindObjectOfType<InputActionManager>();
//     }


//     public void RegisterItem(Item item)
//     {
//         if (!items.Contains(item))
//         {
//             items.Add(item);
//         }
//     }

//     public void RemovePieceCheck (Item item) {
//         if (items.Contains(item)) {
//             items.Remove(item);
//         }
//     }

//     public void CheckWinCondition()
//     {
//         foreach (var item in items)
//         {
//             if (!item.AreAllChecksTrue())
//             {
//                 Debug.Log(item.itemName + " fail the check");
//                 return;
//             }
//         }
//         winConditionMet = true;
//         if (!sketchbookUI.activeSelf){
//             ShowWinText();
//         }

//     }

//     public void OnSketchbookClosed(){
//         if (winConditionMet && !sketchbookUI.activeSelf){
//             ShowWinText();
//             EventSystem.current.SetSelectedGameObject(initButton);
//         }
//     }
//     private void ShowWinText()
//     {
//         inputActionManager.SetPlayerActive(false);
//         if (winText != null)
//         {
//             winText.SetActive(true);
//             Debug.Log("YOU WIN!");
//         }
//         else
//         {
//             Debug.LogWarning("Win text is not assigned in CheckManager.");
//         }
//     }

//     public void BackToMenu()
//     {
//         Resources.Load<Inventory>("PlayerInventory").Clear();
//         SceneManager.LoadScene("StartPage");
//     }


//     public void ContinueExplore()
//     {
//         inputActionManager.SetPlayerActive(true);
//         if (winText != null)
//         {
//             Destroy(winText);
//             winText = null;
//         }
//     }
// }

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

    [Header("Win Condition Actions")]
    public string currentJesterName = "Jester_Animations"; // Name of the current Jester GameObject
    public GameObject newJester;                         // Reference to the new Jester GameObject

    [Header("Circus Tent Replacement")]
    public string currentTentName = "SM_Circus_Tent closed(Clone)"; // Name of the current circus tent in the scene
    public GameObject newTentPrefab;                               // Prefab for the new tent

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

    public void RemovePieceCheck(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
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
        if (!sketchbookUI.activeSelf)
        {
            PerformWinActions();
        }
    }

    public void OnSketchbookClosed()
    {
        if (winConditionMet && !sketchbookUI.activeSelf)
        {
            PerformWinActions();
            // EventSystem.current.SetSelectedGameObject(initButton);
        }
    }

    private void PerformWinActions()
    {
        // inputActionManager.SetPlayerActive(false);

        // Locate and destroy the current Jester in the scene by name
        GameObject currentJester = GameObject.Find(currentJesterName);
        if (currentJester != null)
        {
            Destroy(currentJester);
            Debug.Log("Current Jester destroyed.");
        }
        else
        {
            Debug.LogWarning($"No Jester named '{currentJesterName}' found in the scene.");
        }

        // Activate the new Jester
        if (newJester != null)
        {
            newJester.SetActive(true);
            Debug.Log("New Jester appears!");
        }
        else
        {
            Debug.LogWarning("New Jester GameObject is not assigned.");
        }

        // Locate the current circus tent in the scene by name
        GameObject currentTent = GameObject.Find(currentTentName);

        if (currentTent != null && newTentPrefab != null)
        {
            // Get the current position and rotation of the tent
            Vector3 tentPosition = currentTent.transform.position;
            Quaternion tentRotation = currentTent.transform.rotation;

            // Instantiate the new tent at the same position and rotation
            GameObject newTent = Instantiate(newTentPrefab, tentPosition, tentRotation);

            // Destroy the old tent
            Destroy(currentTent);
            Debug.Log("Replaced the current circus tent with a new one.");
        }
        else
        {
            Debug.LogWarning("Current tent not found or new tent prefab is not assigned.");
        }

        // Display the win text if available
        // if (winText != null)
        // {
        //     winText.SetActive(true);
        //     Debug.Log("YOU WIN!");
        // }
    }

    public void BackToMenu()
    {
        Resources.Load<Inventory>("PlayerInventory").Clear();
        SceneManager.LoadScene("StartPage");
    }

    public void ContinueExplore()
    {
        inputActionManager.SetPlayerActive(true);
        if (winText != null)
        {
            Destroy(winText);
            winText = null;
        }
    }
}
