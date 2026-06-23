using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror_2 : MonoBehaviour
{
    // 🌟 Split into two clean assets in the Inspector!
    [Header("Dialogues")]
    public Dialogue dialoguePart1;
    public Dialogue dialoguePart2;

    [SerializeField] private PlayerInventory _playerInventory;

    [Header("Horror Elements")]
    [SerializeField] private GameObject _flashPhotoB;

    private bool _hasInteracted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasInteracted) return;

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null && inventory.hasGun == true)
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

            if (dialogueManager != null)
            {
                _hasInteracted = true;
                // Run the simple linear sequence
                StartCoroutine(SplitDialogueSequence(dialogueManager));
            }
        }
    }

    private IEnumerator SplitDialogueSequence(DialogueManager dialogueManager)
    {
        // 1. Play Part 1
        dialogueManager.StartDialogue(dialoguePart1);
        yield return null; // Wait for UI initialization

        // 2. Wait completely until Part 1 finishes closing
        while (dialogueManager.isDialogueActive == true)
        {
            yield return null;
        }

        // 3. JUMPSCARE FLASH! (Pauses the script here for 0.5s)
        yield return StartCoroutine(FlashPhotoRoutine());

        // 4. Play Part 2 right after the flash finishes!
        dialogueManager.StartDialogue(dialoguePart2);
    }

    private IEnumerator FlashPhotoRoutine()
    {
        Debug.Log("MIRROR JUMPSCARE ACTIVATED!");
        if (_flashPhotoB != null)
        {
            _flashPhotoB.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _flashPhotoB.SetActive(false);
        }
    }
}