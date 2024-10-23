using UnityEngine;

public class JesterControl : MonoBehaviour
{
    public GameObject jesterOne;  // Reference to Jester-1 in the scene
    public GameObject jesterTwo;  // Prefab for Jester-2 to instantiate
    public GameObject jesterThree;
    private bool existed;
    private int order;

    void Start()
    {
        CheckForSticker();
        existed = false;
        order = 0;
    }

    void Update(){
        if (order == 0){
            CheckForSticker();
        }

        if (order == 1){
            CheckForFountain();
        }
        
    }

    // Method to check for the sticker and swap Jesters if needed
    void CheckForSticker()
    {
        // Find the sticker in the scene using its tag
        GameObject sticker = GameObject.Find("FountainSticker");
        // GameObject fountain = GameObject.Find("SM_Fountain(1)(Clone)");

        // If the sticker is not found, destroy Jester-1 and instantiate Jester-2
        if (sticker == null && existed)
        {

            // Destroy Jester-1
            if (jesterOne != null)
            {
                Destroy(jesterOne);
                order += 1;
            }

            // Instantiate Jester-2 in the scene
            if (jesterTwo != null)
            {
                // Instantiate(jesterTwoPrefab, jesterOne.transform.position, jesterOne.transform.rotation);
                jesterTwo.SetActive(true);
            }
        }
        else
        {   
            if (sticker != null){
                existed = true;
            }
        }
    }


    void CheckForFountain()
    {
        // Find the sticker in the scene using its tag
        
        GameObject fountain = GameObject.Find("SM_Fountain (1)(Clone)");

        // If the sticker is not found, destroy Jester-1 and instantiate Jester-2
        if (fountain)
        {
            // Destroy Jester-1
            if (jesterTwo != null)
            {
                Destroy(jesterTwo);
            }

            // Instantiate Jester-2 in the scene
            if (jesterThree != null)
            {
                // Instantiate(jesterTwoPrefab, jesterOne.transform.position, jesterOne.transform.rotation);
                jesterThree.SetActive(true);
            }
        }
    }
}
