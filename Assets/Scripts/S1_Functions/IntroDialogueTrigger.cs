using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDialogueTrigger : MonoBehaviour
{
    public Dialogue introDialogue; // Drag your opening story text here in the Inspector

    void Start()
    {
        // 1. Find the DialogueManager in the fresh scene
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager != null)
        {
            // 2. Fire the dialogue instantly on frame 1!
            dialogueManager.StartDialogue(introDialogue);
        }
        else
        {
            Debug.LogError("Could not find DialogueManager to start the intro story!");
        }
    }
}
