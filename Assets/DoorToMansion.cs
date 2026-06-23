using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoorTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _doorAnimator;
    [SerializeField] private AudioSource _bangAudioSource;

    [Header("Timing Settings")]
    [SerializeField] private float _delayBeforeDoorOpens = 0.8f; // How long to wait (in seconds) after the bang starts

    private bool _hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            _hasTriggered = true; // Lock it instantly

            // Start the step-by-step timed sequence!
            StartCoroutine(BangThenOpenSequence());
        }
    }

    private IEnumerator BangThenOpenSequence()
    {
        // 1. Play the creepy bang/unlock sound first
        if (_bangAudioSource != null)
        {
            Debug.Log("[Front Trigger] *BANG!* Playing sound effect first.");
            _bangAudioSource.Play();
        }

        // 2. PAUSE the script right here for a fraction of a second
        // You can tweak '_delayBeforeDoorOpens' in the Inspector to time it perfectly with your audio file!
        yield return new WaitForSeconds(_delayBeforeDoorOpens);

        // 3. Open the door automatically after the wait is over!
        if (_doorAnimator != null)
        {
            Debug.Log("[Front Trigger] Delay finished. Automatically opening door!");
            _doorAnimator.SetTrigger("Open");
        }
    }
}