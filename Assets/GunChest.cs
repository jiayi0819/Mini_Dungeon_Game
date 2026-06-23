using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerInventory playerInventory;

    // Two different dialogues you can set up in the Unity Inspector
    [SerializeField] private Dialogue lockedDialogue;
    [SerializeField] private Dialogue openBoxDialogue;

    [Header("Lid Rotation Settings")]
    [SerializeField] private Transform chestLidTransform; // Drag the child lid object here!
    [SerializeField] private float targetXRotation = -70f;  // How far back the lid flips open
    [SerializeField] private float openSpeed = 2f;         // How fast it opens
    private bool isBoxOpened = false;

    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log($"[CHEST] Interact pressed! Box opened? {isBoxOpened}. Player reference valid? {interactor != null}");
        if (isBoxOpened) return false; // If already open, do nothing

        StartCoroutine(BoxInteractionCoroutine());
        return true;
    }
    IEnumerator BoxInteractionCoroutine()
    {
        Debug.Log("1. Coroutine started successfully!");

        // Find the DialogueManager right when we need it
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            yield break;
            Debug.LogError("ERROR: Could not find DialogueManager in the scene!");
        }

            // 1. Check if the player has the key
            if (playerInventory != null && playerInventory.hasKey == true)
        {
            Debug.Log("2. Player HAS the key. Opening chest...");

            // PLAYER HAS KEY -> Open it!
            isBoxOpened = true;
            playerInventory.hasGun = true;
            dialogueManager.StartDialogue(openBoxDialogue);

            // 2. Wait for a quick frame, then smoothly open the lid while they read!
            yield return null;

            // This loop smoothly rotates the X axis until it hits the target angle
            float currentX = chestLidTransform.localEulerAngles.x;
            // localEulerAngles can sometimes jump to 360, so we normalize negative values
            if (currentX > 180) currentX -= 360f;

            while (Mathf.Abs(currentX - targetXRotation) > 0.1f)
            {
                // Move the rotation closer to the target angle frame-by-frame
                currentX = Mathf.MoveTowards(currentX, targetXRotation, openSpeed * Time.deltaTime * 50f);
                chestLidTransform.localRotation = Quaternion.Euler(currentX, 0, 0);

                yield return null; // Wait for the next frame
            }
        }
        else
        {
            Debug.Log($"2. Player does NOT have the key. PlayerInventory status: (Null? {playerInventory == null})");

            if (lockedDialogue == null)
            {
                Debug.LogError("ERROR: You forgot to assign 'lockedDialogue' in the Unity Inspector!");
            }

            // PLAYER DOES NOT HAVE KEY -> Show locked message
            dialogueManager.StartDialogue(lockedDialogue);
        }

        // 3. Keep the interaction locked until they finish reading whatever dialogue played
        yield return null;
        while (dialogueManager.isDialogueActive == true)
        {
            yield return null;
        }

        Debug.Log("Chest interaction complete.");
    }

}
