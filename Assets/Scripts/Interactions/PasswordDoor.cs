using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private string _correctPassword = "0105"; // Set your 4-digit code in Inspector

    [Header("References")]
    [SerializeField] private Animator _leftDoorAnimator;
    [SerializeField] private Animator _rightDoorAnimator;

    public Dialogue dialogue;

    public string InteractionPrompt => _prompt;

    // THE STATE GUARD: Tracks if the door is already unlocked
    private bool _hasOpenedBefore = false;

    public bool Interact(Interactor interactor)
    {
        // RULE 1: If the door is already unlocked, don't show the password menu again!
        if (_hasOpenedBefore)
        {
            Debug.Log("Door is already open. Skipping puzzle sequence.");

            // Optional: If you want pressing E to close the door again later, 
            // you could call a CloseDoor() function here. Otherwise, just return true.
            return true;
        }

        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager != null)
        {
            Debug.Log("Starting Dialogue Sequence...");
            // Instead of running everything at once, we start a Coroutine sequence
            StartCoroutine(DialogueThenPasswordSequence(dialogueManager));
            return true;
        }

        return false;
    }

    private IEnumerator DialogueThenPasswordSequence(DialogueManager dialogueManager)
    {
        // 1. Start the dialogue normally
        dialogueManager.StartDialogue(dialogue);

        // 2. Wait exactly 1 frame just to let the DialogueManager turn on its panels
        yield return null;

        // 3. Keep waiting here AS LONG AS the dialogue box is still open/active
        // NOTE: Replace 'isOpen' with whatever variable your DialogueManager uses 
        // to track if it's currently running (e.g., 'isActive', 'isTalking', etc.)
        while (dialogueManager.isDialogueActive == true)
        {
            yield return null; // Pauses here and checks again next frame
        }

        // 4. The moment the dialogue loop finishes and closes, open the password!
        OpenPasswordPrompt();
    }

    ////////////////////////////////////////////////////
    //  OLD FUNCTIONS 
    ////////////////////////////////////////////////////

    //    //var inventory = interactor.GetComponent<Inventory>();

    //    //if (inventory == null) return false;

    //    //if (inventory.HasKey)
    //    //{
    //    //    Debug.Log("Opening Door!");
    //    //    return true;
    //    //}

    //    //Debug.Log("No Key found!");
    //    //return false;

    private void OpenPasswordPrompt()
    {
        Debug.Log("Oi!");
        if (PasswordMenuManager.Instance != null)
        {
            // We pass the correct password and the function we want to run on success
            PasswordMenuManager.Instance.ShowPasswordMenu(_correctPassword, OpenDoor);
        }
        else
        {
            Debug.LogError("PasswordMenuManager instance not found in scene!");
        }
    }

    // This runs ONLY when PasswordMenuManager confirms the password matches
    private void OpenDoor()
    {
        Debug.Log("The password was correct! Opening the door animation/logic goes here.");


        // RULE 2: Lock it in! Set our tracker to true so the password menu never spawns again.
        _hasOpenedBefore = true;

        // Add your actual door opening code here (e.g., Animator trigger or destroying the object)
        // Destroy(gameObject); // Temporary placeholder
        
        
        // 1. TRIGGER DOOR ANIMATIONS

        if (_leftDoorAnimator != null)
        {
            Debug.Log("Open Left Door!!!");
            _leftDoorAnimator.SetTrigger("OpenLeft");
        }
        if (_rightDoorAnimator != null)
        {
            _rightDoorAnimator.SetTrigger("OpenRight");
        }
    }
}
