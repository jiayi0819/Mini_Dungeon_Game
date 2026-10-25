using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairwellTrigger : MonoBehaviour
{
    public Dialogue dialogueAfterSlam;

    [Header("References")]
    [SerializeField] private Animator _doorAnimator;

    private bool _hasSlammed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasSlammed) return;

        if (other.CompareTag("Player"))
        {
            _hasSlammed = true; // Lock it so it doesn't slam repeatedly

            // 1. Slam the door closed automatically
            if (_doorAnimator != null)
            {
                Debug.Log("[Stairwell Trigger] Player entered! Slamming door shut.");
                _doorAnimator.SetTrigger("Close"); // Match your Animator's close trigger parameter name
            }

            // 2. Fire the dialogue sequence right after the slam
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            if (dialogueManager != null && dialogueAfterSlam != null)
            {
                dialogueManager.StartDialogue(dialogueAfterSlam);
            }
        }
    }
}