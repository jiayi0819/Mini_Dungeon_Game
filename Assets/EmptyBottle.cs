using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBottle : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    public Dialogue dialogue;

    public string InteractionPrompt => _prompt;

    [Header("Fainting Setup")]
    [SerializeField] private SceneFadeIn _fader;         // Drag your Black Image UI object here
    [SerializeField] private Transform _wakeUpPoint;     // Drag an empty GameObject placed in the new room
    [SerializeField] private Transform _playerTransform; // Drag your Player object here

    private bool _hasTriggered = false; // Prevents the player from spamming the mirror

    public bool Interact(Interactor interactor)
    {
        if (_hasTriggered) return false;

        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager != null)
        {
            _hasTriggered = true; // Lock it so it only happens once

            // Start the sequence that handles both dialogue and fainting
            StartCoroutine(DialogueThenFaintSequence(dialogueManager));
            return true;
        }

        return false;
    }

    private IEnumerator DialogueThenFaintSequence(DialogueManager dialogueManager)
    {
        // 1. Start the dialogue box normally
        dialogueManager.StartDialogue(dialogue);

        // 2. Wait 1 frame to let DialogueManager initialize panels
        yield return null;

        // 3. Keep waiting here AS LONG AS the player is reading the dialogue
        while (dialogueManager.isDialogueActive == true)
        {
            yield return null; // Check again next frame
        }

        // 4. The moment the text box closes... BLACKOUT!
        Debug.Log("Dialogue finished! Initiating faint sequence.");

        if (_fader != null && _wakeUpPoint != null && _playerTransform != null)
        {
            _fader.TriggerFaint(_playerTransform, _wakeUpPoint);
        }

        // Disable this script component completely so they can never interact again
        this.enabled = false;
    }
}