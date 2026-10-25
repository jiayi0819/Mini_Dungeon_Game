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
                // Get the text first and check if it's blank!
                string promptText = _interactable.InteractionPrompt;

                if (string.IsNullOrEmpty(promptText))
                {
                    // If the mirror says "", hide the panel and do not allow interaction!
                    if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
                }
                else
                {
                    if (!_interactionPromptUI.IsDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);


                    if (Keyboard.current.eKey.wasPressedThisFrame)
                    {
                        if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
                        _interactable.Interact(this);

                    }
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
