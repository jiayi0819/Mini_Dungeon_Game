using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JekyllDiary : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    public Dialogue dialogue;
    public bool HasInteracted => _hasTriggered;

    private bool _hasTriggered = false;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {

        // Find the DialogueManager and feed it this sign's unique dialogue data
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager != null)
        {
            _hasTriggered = true;
            dialogueManager.StartDialogue(dialogue);
            return true;
        }

        return false;
    }
}
