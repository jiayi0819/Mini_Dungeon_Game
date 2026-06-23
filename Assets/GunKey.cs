using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunKey : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private PlayerInventory playerInventory;
    public Dialogue dialogue;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {

        // Find the DialogueManager and feed it this sign's unique dialogue data
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager != null)
        {
            playerInventory.hasKey = true;
            dialogueManager.StartDialogue(dialogue);
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
