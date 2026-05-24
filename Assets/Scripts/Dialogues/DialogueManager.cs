using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    
    public Animator animator;

    //private Queue<string> sentences;
    private Queue<DialogueLine> lines;

    // To track if a dialogue box is currently open on screen
    public bool isDialogueActive = false;

    // Start is called before the first frame update
    void Start()
    {
        lines = new Queue<DialogueLine>();
    }

    // This handles the keyboard listening every frame
    private void Update()
    {
        // If no dialogue is on screen, do absolutely nothing!
        if (!isDialogueActive) return;

        // Check if the player pressed Space, Enter, or Left Click
        if (Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue (Dialogue dialogue)
    {
        // To trigger dialogue box animation
        animator.SetBool("IsOpen", true);

        // As flag for checking keyboard input
        isDialogueActive = true;

        lines.Clear();

        foreach (DialogueLine line in dialogue.dialogueLines)
        {
            lines.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
        DialogueLine currentLine = lines.Dequeue();

        nameText.text = currentLine.speakerName; // Updates the speaker name dynamically per line!
        dialogueText.text = currentLine.sentence;   // Updates the text line
    }
    void EndDialogue()
    {
        // To trigger dialogue box animation
        animator.SetBool("IsOpen", false);
        isDialogueActive = false;
    }

}
