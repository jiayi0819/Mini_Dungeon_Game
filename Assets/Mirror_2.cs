using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Look Ma, no IInteractable!
public class Mirror_2 : MonoBehaviour
{
    public Dialogue dialogue;
    private bool hasInteracted = false;
    [SerializeField] private PlayerInventory playerInventory;

    private void OnTriggerEnter(Collider other)
    {
        if (hasInteracted) return;

        // Check if the player stepped into the zone
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null && playerInventory.hasGun == true)
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

            if (dialogueManager != null)
            {
                hasInteracted = true;
                dialogueManager.StartDialogue(dialogue);
            }
        }
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Mirror_2 : MonoBehaviour, IInteractable
//{
//    [SerializeField] private string _prompt;
//    [SerializeField] private PlayerInventory playerInventory;
//    public Dialogue dialogue;
//    private bool hasInteracted = false;

//    public string InteractionPrompt
//    {
//        get
//        {
//            // 1. If it already ran once, hide the prompt completely
//            if (hasInteracted) return "";

//            // 2. Find the player's inventory automatically
//            PlayerInventory inventory = FindObjectOfType<PlayerInventory>();

//            // 3. CRITICAL: If they don't have the item, return an empty string so NO prompt shows up
//            if (inventory == null || playerInventory.hasGun == false)
//            {
//                return "";
//            }

//            // 4. Only show the prompt if they have the item and haven't used it yet
//            return _prompt;
//        }
//    }
//        public bool Interact(Interactor interactor)
//    {
//        // Double security: block interaction if already used or if they don't have the item
//        if (hasInteracted) return false;

//        if (playerInventory == null || playerInventory.hasGun == false)
//        {
//            return false; // Exit silently, do absolutely nothing
//        }

//        // --- SUCCESS: Run the dialogue once ---
//        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

//        if (dialogueManager != null)
//        {
//            hasInteracted = true; // Lock it forever right here

//            dialogueManager.StartDialogue(dialogue);
//            return true;
//        }

//        return false;
//    }
//}
