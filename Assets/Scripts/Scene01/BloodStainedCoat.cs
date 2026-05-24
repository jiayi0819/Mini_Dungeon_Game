using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStainedCoat : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    public Dialogue dialogue;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {

        // Find the DialogueManager and feed it this sign's unique dialogue data
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogue);
            return true;
        }

        return false;
    }
}
