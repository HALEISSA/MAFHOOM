using UnityEngine;
using System.Collections;

public class FinalMessageManager : MonoBehaviour
{
    public GameObject finalMessagePanel;
    public CanvasGroup canvasGroup;

    public float fadeDuration = 0.8f;
    public float startScale = 0.85f;
    public float endScale = 1f;

    public void ShowFinalMessage()
    {
        if (finalMessagePanel == null || canvasGroup == null)
        {
            Debug.LogError("FinalMessageManager references are missing.");
            return;
        }

        finalMessagePanel.SetActive(true);
        StartCoroutine(AnimateMessage());
    }

    private IEnumerator AnimateMessage()
    {
        float time = 0f;

        canvasGroup.alpha = 0f;
        finalMessagePanel.transform.localScale = Vector3.one * startScale;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            finalMessagePanel.transform.localScale = Vector3.Lerp(
                Vector3.one * startScale,
                Vector3.one * endScale,
                t
            );

            yield return null;
        }

        canvasGroup.alpha = 1f;
        finalMessagePanel.transform.localScale = Vector3.one * endScale;
    }
}