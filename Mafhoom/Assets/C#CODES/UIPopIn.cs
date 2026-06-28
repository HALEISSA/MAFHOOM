using System.Collections;
using UnityEngine;

public class UIPopIn : MonoBehaviour
{
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float overshoot = 1.03f;

    private Vector3 startScale;
    private Vector3 endScale;

    private void Awake()
    {
        startScale = transform.localScale;
        endScale = Vector3.one;
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(Pop());
    }

    private IEnumerator Pop()
    {
        float t = 0f;

        // scale up to overshoot
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = t / duration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.one * overshoot, EaseOutCubic(k));
            yield return null;
        }

        // settle back to 1
        t = 0f;
        while (t < duration * 0.6f)
        {
            t += Time.unscaledDeltaTime;
            float k = t / (duration * 0.6f);
            transform.localScale = Vector3.Lerp(Vector3.one * overshoot, endScale, EaseOutCubic(k));
            yield return null;
        }

        transform.localScale = endScale;
    }

    private float EaseOutCubic(float x) => 1f - Mathf.Pow(1f - x, 3f);
}
