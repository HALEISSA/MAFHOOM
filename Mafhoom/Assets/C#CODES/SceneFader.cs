using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.35f;

    private static SceneFader _instance;
    private Image _overlay;
    private bool _isTransitioning;

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); return; }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        BuildOverlay();
    }

    private void BuildOverlay()
    {
        var canvasGO = new GameObject("FadeCanvas");
        canvasGO.transform.SetParent(transform);

        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        var imgGO = new GameObject("FadeImage");
        imgGO.transform.SetParent(canvas.transform, false);

        var rt = imgGO.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;

        _overlay = imgGO.AddComponent<Image>();
        _overlay.color = new Color(0f, 0f, 0f, 0f);
        _overlay.raycastTarget = false;
    }

    public void FadeToScene(string sceneName)
    {
        if (_isTransitioning) return;
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        _isTransitioning = true;
        yield return Fade(0f, 1f);
        SceneManager.LoadScene(sceneName);
        yield return null;
        yield return Fade(1f, 0f);
        _isTransitioning = false;
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(from, to, Mathf.Clamp01(t / fadeDuration)));
            yield return null;
        }
        SetAlpha(to);
    }

    private void SetAlpha(float a)
    {
        if (_overlay == null) return;
        var c = _overlay.color;
        c.a = a;
        _overlay.color = c;
    }
}
