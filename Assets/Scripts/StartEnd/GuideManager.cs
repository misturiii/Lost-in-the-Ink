using UnityEngine;
using UnityEngine.EventSystems;

public class PageManager : MonoBehaviour
{
    // References to the GameObjects for Page and GuidePage
    public GameObject page;
    public GameObject guidePage;
    [SerializeField] GameObject button;
    [SerializeField] GameObject backButton;
    
    EventSystem eventSystem;

    void Start () {
        eventSystem = EventSystem.current;
    }

    // This method shows the GuidePage and hides the Page
    public void ShowGuidePage()
    {
        page.SetActive(false);        // Hide the main page
        guidePage.SetActive(true);    // Show the guide page
        button = eventSystem.currentSelectedGameObject;
        if (backButton) {
            eventSystem.SetSelectedGameObject(backButton);
        }
    }

    // This method shows the Page and hides the GuidePage
    public void ShowMainPage()
    {
        guidePage.SetActive(false);   // Hide the guide page
        page.SetActive(true);         // Show the main page
        if (button) {
            eventSystem.SetSelectedGameObject(button);
        }
    }
}
