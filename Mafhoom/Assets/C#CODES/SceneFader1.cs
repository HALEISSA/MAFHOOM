using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader1 : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.35f;

    private void Awake()
    {
        if (fadeImage == null) fadeImage = GetComponent<Image>();
    }

    private void Start()
    {
        // Start black -> fade to transparent
        StartCoroutine(Fade(1f, 0f));
    }

    public void FadeOutThenLoad(string sceneName)
    {
        StartCoroutine(FadeOutLoad(sceneName));
    }

    private IEnumerator FadeOutLoad(string sceneName)
    {
        yield return Fade(0f, 1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(from, to, t / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        fadeImage.color = new Color(c.r, c.g, c.b, to);
    }
}
