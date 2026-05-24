using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    private Image _fadeImage;
    public float fadeSpeed = 0.1f;

    void Start()
    {
        _fadeImage = GetComponent<Image>();
        StartCoroutine(FadeOutCurtain());
    }

    private IEnumerator FadeOutCurtain()
    {
        float alpha = 1.0f;

        // Gradually reduce transparency until it's invisible
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            _fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null; // Wait for the next frame
        }

        // Destroy the overlay so it doesn't block player clicks
        Destroy(gameObject);
    }
}