using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{

    private Animator _doorAnimator; //Animator component on the door

    
    void Start()
    {
        _doorAnimator = GetComponent<Animator>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if the collider belongs to the player
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        //if (other.CompareTag("Player"))
        {
            _doorAnimator.SetTrigger("Open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Optional : Automatically close the door when player leaves trigger area
        //if (other.CompareTag("Player"))
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            _doorAnimator.SetTrigger("Closed");
        }
    }
}
