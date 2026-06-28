using System.Collections;
using TMPro;
using UnityEngine;

public class PuzzleSceneIntroController : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public GameObject puzzleUI;
    public AudioSource audioSource;

    public float fadeDuration = 0.8f;
    public float voiceEndTime = 3.2f; // ⬅️ we’ll adjust this if needed

    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        puzzleUI.SetActive(false);

        introText.gameObject.SetActive(true);
        SetTextAlpha(0);

        // Fade IN
        yield return StartCoroutine(FadeText(0, 1));

        // Wait until voice line finishes
        while (audioSource.time < voiceEndTime)
        {
            yield return null;
        }

        // Fade OUT
        yield return StartCoroutine(FadeText(1, 0));

        introText.gameObject.SetActive(false);

        // Show puzzle exactly when music starts
        puzzleUI.SetActive(true);
    }

    IEnumerator FadeText(float start, float end)
    {
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start, end, t / fadeDuration);
            SetTextAlpha(a);
            yield return null;
        }

        SetTextAlpha(end);
    }

    void SetTextAlpha(float a)
    {
        Color c = introText.color;
        c.a = a;
        introText.color = c;
    }
}