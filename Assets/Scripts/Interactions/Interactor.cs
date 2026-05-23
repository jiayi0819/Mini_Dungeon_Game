using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Script attached to Player object

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;
    [SerializeField] private DialogueManager _dialogueManager; //to check if dialogue is there

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    private IInteractable _interactable;

    private void Update()
    {

        // Close Prompt Text Panel if Dialogue already started
        if (_dialogueManager != null && _dialogueManager.isDialogueActive)
        {
            if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
            return;
        }

        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();

            if (_interactable != null)
            {
                Debug.Log("Show prompt panel.");

                if (!_interactionPromptUI.IsDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);

                
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
                    _interactable.Interact(this);

                    //DialogueTrigger objectDialogue = _colliders[0].GetComponent<DialogueTrigger>();

                    // 2. If the object DOES have the script, run its dialogue!
                    //if (objectDialogue != null)
                    //{
                    //    if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
                    //    objectDialogue.TriggerDialogue();
                    //}
                }
            }
        }else
        {
            if (_interactable != null) _interactable = null;
            if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
