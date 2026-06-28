using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private float fadeDuration = 0.35f;

    private bool isTransitioning;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (fadeOverlay != null)
        {
            var c = fadeOverlay.color;
            c.a = 0f;
            fadeOverlay.color = c;
        }
    }

    public void FadeToScene(string sceneName)
    {
        if (isTransitioning) return;
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        isTransitioning = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Clamp01(t / fadeDuration));
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    private void SetAlpha(float a)
    {
        if (fadeOverlay == null) return;
        var c = fadeOverlay.color;
        c.a = a;
        fadeOverlay.color = c;
    }
}
