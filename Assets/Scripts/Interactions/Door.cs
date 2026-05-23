using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
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

        //var inventory = interactor.GetComponent<Inventory>();

        //if (inventory == null) return false;

        //if (inventory.HasKey)
        //{
        //    Debug.Log("Opening Door!");
        //    return true;
        //}

        //Debug.Log("No Key found!");
        //return false;
    }
}
