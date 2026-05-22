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

    private Queue<string> sentences;
    
    // To track if a dialogue box is currently open on screen
    private bool isDialogueActive = false;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
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
        nameText.text = dialogue.name;

        // To trigger dialogue box animation
        animator.SetBool("IsOpen", true);

        // As flag for checking keyboard input
        isDialogueActive = true;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }
    void EndDialogue()
    {
        // To trigger dialogue box animation
        animator.SetBool("IsOpen", false);
        isDialogueActive = false;
    }

}
