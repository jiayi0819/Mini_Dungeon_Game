using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string InteractionPrompt { get; }

    // NEW: Everyone implementing this interface must expose their interaction status!
    public bool HasInteracted { get; }

    public bool Interact(Interactor interactor);
}