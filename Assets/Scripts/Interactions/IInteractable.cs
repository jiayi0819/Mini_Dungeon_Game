using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string InteractionPrompt { get; }

    public bool Interact(Interactor interactor);
}

///////////////////////////////////////////////////
// FOR ITEMS WITH DIALOGUE
////////////////////////////////////////////////////

// 1. Add Variable
//public Dialogue dialogue;

// 2. Add below in Interact() function

//DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

//if (dialogueManager != null)
//{
//    dialogueManager.StartDialogue(dialogue);
//    return true;
//}

//return false;