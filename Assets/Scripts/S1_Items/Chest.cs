using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{

    [SerializeField] private string _prompt;
    public Dialogue dialogue;
    public bool HasInteracted => _hasTriggered;

    private bool _hasTriggered = false;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Opening Chest!");
        _hasTriggered = true;
        return true;
    }
}
