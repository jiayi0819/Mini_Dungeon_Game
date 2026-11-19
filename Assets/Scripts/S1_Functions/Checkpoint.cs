
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Required Objects (Must implement IInteractable)")]
    [SerializeField] private List<MonoBehaviour> _requiredObjects = new List<MonoBehaviour>();

    [Header("Push Back & Dialogue")]
    [SerializeField] private Transform _pushBackPoint;
    [SerializeField] private float _pushBackDistance = 1.5f;

    public Dialogue incompleteDialogue;
    public Dialogue completedDialogue;

    [SerializeField] private float _cooldownSeconds = 1.5f;
    [SerializeField] private bool _allowFirstEntry;

    private bool _hasEnteredRoom = false;
    private bool _isUnlocked = false;
    private bool _isOnCooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        Interactor interactor = other.GetComponentInParent<Interactor>();

        if (interactor == null && !other.CompareTag("Player"))
            return;

        if (_isUnlocked)
            return;

        // First entry handling
        if (!_hasEnteredRoom)
        {
            Debug.Log("Never enter the room");
            _hasEnteredRoom = true;

            if (_allowFirstEntry)
            {
                Debug.Log("Should allow first entry");
                return;
            }
        }

        // Subsequent entry means the player is trying to leave.
        if (HasCompletedAllRequired())
        {
            _isUnlocked = true;

            if (completedDialogue != null)
            {
                DialogueManager dm = FindObjectOfType<DialogueManager>();

                if (dm != null && !dm.isDialogueActive)
                    dm.StartDialogue(completedDialogue);
            }

            return;
        }

        // Requirements incomplete: Push player back inside.
        PushPlayerBack(other, interactor);

        if (!_isOnCooldown)
            TriggerIncompleteDialogue();
    }

    private void PushPlayerBack(Collider other, Interactor interactor)
    {
        Transform playerRoot = interactor != null
            ? interactor.transform
            : other.transform.root;

        CharacterController cc = playerRoot.GetComponent<CharacterController>();

        if (_pushBackPoint != null)
        {
            if (cc != null)
                cc.enabled = false;

            playerRoot.position = _pushBackPoint.position;

            if (cc != null)
                cc.enabled = true;
        }
        else
        {
            Vector3 pushDir = -transform.forward.normalized;

            if (cc != null)
                cc.Move(pushDir * _pushBackDistance);
            else
                playerRoot.position += pushDir * _pushBackDistance;
        }
    }

    private bool HasCompletedAllRequired()
    {
        foreach (MonoBehaviour item in _requiredObjects)
        {
            if (item == null)
                continue;

            if (item is IInteractable interactable)
            {
                Debug.Log(item + " & " + interactable.HasInteracted);
                if (!interactable.HasInteracted)
                    return false;
            }
            else
            {
                Debug.LogWarning(
                    $"{item.name} in _requiredObjects does not implement IInteractable!",
                    this
                );
            }
        }

        return true;
    }

    private void TriggerIncompleteDialogue()
    {
        DialogueManager dm = FindObjectOfType<DialogueManager>();

        if (dm != null && incompleteDialogue != null && !dm.isDialogueActive)
        {
            dm.StartDialogue(incompleteDialogue);
            StartCoroutine(CooldownRoutine());
        }
    }

    private IEnumerator CooldownRoutine()
    {
        _isOnCooldown = true;

        yield return new WaitForSeconds(_cooldownSeconds);

        _isOnCooldown = false;
    }
}
