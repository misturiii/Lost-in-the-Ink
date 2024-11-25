using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class DynamicGuideManager : MonoBehaviour
{
    public Image guideImage;  // Reference to the guide image UI element
    public Sprite[] guideSprites;  // Array of guide sprites to display
    public float shrinkDuration = 1.5f;  // Duration to shrink the image
    public float timeBeforeMove = 1f;  // Time to wait before shrinking and moving the guide
    public float moveDuration = 1.5f;  // Duration over which the guide moves to the left
    public InputActionAsset inputActionAsset;  // Reference to your InputActionAsset

    private int currentGuideIndex = 0;  // Track the current guide sprite index
    private InputAction moveAction;  // InputAction for movement
    private InputAction jumpAction;  // InputAction for jump

    private Vector3 originalSize;  // Variable to store the original size of the guide image

    private void OnEnable()
    {
        if (inputActionAsset != null)
        {
            // Find the action from your InputActionAsset for movement and jumping
            moveAction = inputActionAsset.FindAction("Player/Move");  // Adjust the path as per your InputActionAsset structure
            jumpAction = inputActionAsset.FindAction("Player/Jump");  // Adjust the path for the jump action

            moveAction.Enable();  // Enable the move action
            jumpAction.Enable();  // Enable the jump action
        }
        else
        {
            Debug.LogError("InputActionAsset is not assigned!");
        }
    }

    private void OnDisable()
    {
        // Disable the actions when the object is disabled
        if (moveAction != null)
        {
            moveAction.Disable();
        }
        if (jumpAction != null)
        {
            jumpAction.Disable();
        }
    }

    private void Start()
    {
        // Make sure the guide image is visible
        if (guideImage != null && guideSprites.Length > 0)
        {
            // Store the original size of the guide image (at the start)
            originalSize = guideImage.transform.localScale;

            // Show the first guide
            ShowGuide(guideSprites[currentGuideIndex]);
        }
        else
        {
            Debug.LogWarning("Guide image or sprites are not assigned!");
        }

        // Listen for the input action triggers to move to the next guide (e.g., when the Move or Jump action is triggered)
        moveAction.started += OnMoveActionPerformed;
        jumpAction.started += OnJumpActionPerformed;

        // Start monitoring for the Fountain Sticker appearance
        StartCoroutine(CheckForFountainSticker());
       
    }

    private void OnDestroy()
    {
        // Unsubscribe from the input action events to avoid memory leaks
        if (moveAction != null)
        {
            moveAction.started -= OnMoveActionPerformed;
        }
        if (jumpAction != null)
        {
            jumpAction.started -= OnJumpActionPerformed;
        }
    }

    // This is triggered when the move action is performed (e.g., player starts moving)
    private void OnMoveActionPerformed(InputAction.CallbackContext context)
    {
        if (currentGuideIndex == 0)
        {
            ShowNextGuide();
        }
    }

    // This is triggered when the jump action is performed (e.g., player presses the jump button)
    private void OnJumpActionPerformed(InputAction.CallbackContext context)
    {
        if (currentGuideIndex == 1)
        {
            ShowNextGuide();
        }
    }

    private void ShowGuide(Sprite guideSprite)
    {
        StartCoroutine(DelayedShowGuide(guideSprite));
    }

    private IEnumerator DelayedShowGuide(Sprite guideSprite)
    {   
        if(currentGuideIndex != 0){
            yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds

        }
        guideImage.sprite = guideSprite;  // Set the current sprite
        guideImage.gameObject.SetActive(true);  // Make sure the guide is visible

        // Position the guide in the center of the screen initially
        guideImage.rectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Reset the scale to the original size before displaying the guide
        guideImage.transform.localScale = originalSize;

        // Start the process of showing, shrinking, and moving the guide
        StartCoroutine(ShowAndMoveGuide());
    }


    // Show the next guide in the sequence
    // private void ShowNextGuide()
    // {
    //     // Hide the current guide and increment the index
    //     guideImage.gameObject.SetActive(false);
    //     currentGuideIndex++;

    //     if (currentGuideIndex < guideSprites.Length)
    //     {
    //         ShowGuide(guideSprites[currentGuideIndex]);
    //     }
    //     else
    //     {
    //         Debug.Log("All guides have been shown.");
    //     }
    // }

    private void ShowNextGuide()
    {
        // Hide the current guide and increment the index
        guideImage.gameObject.SetActive(false);
        currentGuideIndex++;

        if (currentGuideIndex < guideSprites.Length)
        {
            // Show the current guide sprite
            ShowGuide(guideSprites[currentGuideIndex]);

            // Immediately show the next guide if current index is 5
            if (currentGuideIndex == 5)
            {
                // Wait until guide 5's movement and shrink has completed, then show guide 6
                StartCoroutine(WaitForGuideToFinishAndShowNext());
            }
        }
        else
        {
            Debug.Log("All guides have been shown.");
        }
    }

    private IEnumerator WaitForGuideToFinishAndShowNext()
    {
        // Wait until the current guide has finished showing and moving
        yield return new WaitForSeconds(timeBeforeMove + moveDuration + 1); // Wait for the guide's shrink and move duration
        
        // Now show guide 6 immediately after guide 5
        currentGuideIndex++;  // Increment to show the next guide
        ShowGuide(guideSprites[currentGuideIndex]);
    }


    // Coroutine to monitor the appearance of the Fountain Sticker in the scene
   private IEnumerator CheckForFountainSticker()
    {
        while (true)
        {
            GameObject sticker = GameObject.Find("FountainSticker");
            if (sticker)
            {
                Debug.Log("Fountain Sticker has appeared in the scene!");
                if (currentGuideIndex == 2)  // Assuming index 2 is related to the fountain sticker
                {
                    ShowNextGuide();
                }
                
                StartCoroutine(CheckForJester());  // Ensure `CheckForJester` starts after detecting the fountain
                yield break;  // Exit this coroutine since the sticker was found
            }
            yield return new WaitForSeconds(0.5f);  // Check every 0.5 seconds
        }
    }



    private IEnumerator CheckForJester()
    {
        while (true)
        {
            GameObject jester = GameObject.Find("Jester_Animations");
            if (jester)
            {
              

                NPC jesterScript = jester.GetComponent<NPC>();

                if (jesterScript && (jesterScript.dialogueName == "Jester-Map"))
                {
                    
                    if (currentGuideIndex == 3)  // Assuming index 3 is related to the fountain sticker
                    {
                        ShowNextGuide();
                    }
                }
                if (jesterScript && (jesterScript.dialogueName == "Jester-ToolFuntion"))
                {
                    if (currentGuideIndex == 4)  // Assuming index 4 is related to the fountain sticker
                    {
                        ShowNextGuide();
                    }
                    yield break;  // Exit this coroutine as its task is complete
                }
            }
            else
            {
                Debug.Log("Jester not found yet.");
            }

            yield return new WaitForSeconds(0.5f);  // Check every 0.5 seconds
        }
    }


    // Coroutine to show, shrink, and move the guide image
    private IEnumerator ShowAndMoveGuide()
    {
        // Wait for the specified time before shrinking and moving
        yield return new WaitForSeconds(timeBeforeMove);

        // Start shrinking and moving the guide simultaneously
        StartCoroutine(ShrinkGuide());
        StartCoroutine(MoveGuideSmoothlyToLeft());

        yield return new WaitForSeconds(shrinkDuration + moveDuration + 4);  
        // If it's the last guide, hide the image
        if (currentGuideIndex >= guideSprites.Length - 1)
        {
            Destroy(guideImage.gameObject);  // Hide the guide image after the last guide
        }
    }

    // Shrink the guide image to 75% size
    private IEnumerator ShrinkGuide()
    {
        Vector3 targetScale = originalSize * 0.6f;  // Shrink to 60% of the original size
        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            guideImage.transform.localScale = Vector3.Lerp(originalSize, targetScale, elapsedTime / shrinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        guideImage.transform.localScale = targetScale;  // Ensure it reaches the target size
    }

    // Smoothly move the guide image to the left side of the screen
    private IEnumerator MoveGuideSmoothlyToLeft()
    {
        // Get the starting position of the guide image
        Vector3 startPosition = guideImage.rectTransform.position;

        // Calculate the target position at the bottom-left of the screen
        Vector3 targetPosition = new Vector3(150, 150, startPosition.z); // Adjust y for the bottom position

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            guideImage.rectTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the guide image reaches the target position
        guideImage.rectTransform.position = targetPosition;
    }
}
