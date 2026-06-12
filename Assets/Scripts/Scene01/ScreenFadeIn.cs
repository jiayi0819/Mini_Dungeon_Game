using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    private Image _fadeImage;
    public float fadeSpeed = 0.5f; // Made it slightly faster, tweak in inspector

    void Awake()
    {
        _fadeImage = GetComponent<Image>();
    }

    void Start()
    {
        // Start the game by fading out the initial black screen
        StartCoroutine(FadeOutCurtain());
    }

    // --- 1. THE FADE OUT ROUTINE ---
    private IEnumerator FadeOutCurtain()
    {
        float alpha = _fadeImage.color.a;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            _fadeImage.color = new Color(0f, 0f, 0f, Mathf.Max(alpha, 0f));
            yield return null;
        }

        // REMOVED Destroy(gameObject) so we can reuse the screen later!
        // Instead, we just turn off the image component so it doesn't block clicks
        _fadeImage.enabled = false;
    }

    // --- 2. THE FADE IN ROUTINE ---
    private IEnumerator FadeInCurtain()
    {
        _fadeImage.enabled = true; // Turn the image back on
        float alpha = _fadeImage.color.a;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            _fadeImage.color = new Color(0f, 0f, 0f, Mathf.Min(alpha, 1f));
            yield return null;
        }
    }

    // --- 3. THE PUBLIC FAINT SEQUENCE TO CALL FROM YOUR DOOR ---
    public void TriggerFaint(Transform player, Transform wakeUpPoint)
    {
        StartCoroutine(FaintSequence(player, wakeUpPoint));
    }

    private IEnumerator FaintSequence(Transform player, Transform wakeUpPoint)
    {
        // Phase A: Fade to black
        yield return StartCoroutine(FadeInCurtain());

        // Wait 1 second in pure darkness while unconscious
        yield return new WaitForSeconds(1f);

        // Phase B: Teleport (Crucial Starter Asset fix!)
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false; // Turn off controller so it doesn't snap back

        player.position = wakeUpPoint.position;
        player.rotation = wakeUpPoint.rotation;

        if (cc != null) cc.enabled = true;  // Turn it back on safely

        // Phase C: Wake up! Fade out black screen
        yield return StartCoroutine(FadeOutCurtain());
    }
}