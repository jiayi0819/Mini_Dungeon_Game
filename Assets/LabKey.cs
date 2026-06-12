using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryKey : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "Pick up key";
    public Dialogue dialogue;

    [Header("Door Setup")]
    [SerializeField] private Transform _doorTransform; // Drag the door object (or hinge) here
    [SerializeField] private float _openSpeed = 2f;     // How fast the door swings open

    public string InteractionPrompt => _prompt;
    private bool _hasInteracted = false;

    public bool Interact(Interactor interactor)
    {
        if (_hasInteracted) return false;

        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        if (dialogueManager != null)
        {
            _hasInteracted = true;
            StartCoroutine(DialogueThenOpenSequence(dialogueManager));
            return true;
        }

        return false;
    }

    private IEnumerator DialogueThenOpenSequence(DialogueManager dialogueManager)
    {
        // 1. Show the pickup dialogue
        dialogueManager.StartDialogue(dialogue);

        yield return null;

        // 2. Wait until they close the text box
        while (dialogueManager.isDialogueActive == true)
        {
            yield return null;
        }

        // 3. Smoothly rotate the door from -90 to 0
        Debug.Log("Dialogue finished! Swinging door open...");

        if (_doorTransform != null)
        {
            // We run a second mini-coroutine to handle the smooth movement
            yield return StartCoroutine(SmoothRotateDoor(_doorTransform, 0f));

            // Turn off any colliders or interactables on the door parent so it's fully bypassed
            if (_doorTransform.TryGetComponent<BoxCollider>(out BoxCollider col))
            {
                col.enabled = false;
            }
        }

        // 4. Destroy the key itself so it vanishes from the scene
        Destroy(gameObject);
    }

    private IEnumerator SmoothRotateDoor(Transform door, float targetYRotation)
    {
        // Keep track of the current angles
        Vector3 currentRotation = door.localEulerAngles;

        // Unity reads -90 degrees as 270 degrees internally, so we adjust for it
        float currentY = currentRotation.y > 180 ? currentRotation.y - 360 : currentRotation.y;

        float time = 0;
        float duration = 1f / _openSpeed; // Scales time based on speed setting

        while (time < 1f)
        {
            time += Time.deltaTime * _openSpeed;

            // Smoothly blend from current Y angle to the target Y angle (0)
            float newY = Mathf.Lerp(currentY, targetYRotation, time);

            door.localRotation = Quaternion.Euler(currentRotation.x, newY, currentRotation.z);
            yield return null;
        }

        // Ensure it snaps perfectly to exactly 0 at the end
        door.localRotation = Quaternion.Euler(currentRotation.x, targetYRotation, currentRotation.z);
    }
}