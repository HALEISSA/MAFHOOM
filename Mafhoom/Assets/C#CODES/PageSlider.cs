using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PageSlider : MonoBehaviour
{
    // ✅ This remembers onboarding only during the current game session
    private static bool onboardingSeenThisSession = false;

    [Header("Sliding")]
    public RectTransform pagesContainer;
    public int totalPages = 3;
    public float pageWidth = 952f;
    public float speed = 8f;

    [Header("Buttons")]
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject startButton;

    [Header("Panels")]
    public GameObject onboardingPanel;
    public GameObject buttonsRow;

    [Header("Onboarding Fade")]
    public Image onboardingFadeOverlay;
    public float fadeSpeed = 2f;

    private int currentPage = 0;
    private Vector2 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        // If onboarding already seen → skip it
        if (onboardingSeenThisSession)
        {
            onboardingPanel.SetActive(false);
            buttonsRow.SetActive(true);
            return;
        }

        // First time → show onboarding
        onboardingPanel.SetActive(true);
        buttonsRow.SetActive(false);

        currentPage = 0;
        pagesContainer.anchoredPosition = Vector2.zero;
        targetPosition = pagesContainer.anchoredPosition;

        UpdateButtons();

        // Ensure fade starts transparent
        if (onboardingFadeOverlay != null)
        {
            Color c = onboardingFadeOverlay.color;
            c.a = 0f;
            onboardingFadeOverlay.color = c;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            pagesContainer.anchoredPosition = Vector2.Lerp(
                pagesContainer.anchoredPosition,
                targetPosition,
                Time.deltaTime * speed
            );

            if (Vector2.Distance(pagesContainer.anchoredPosition, targetPosition) < 0.1f)
            {
                pagesContainer.anchoredPosition = targetPosition;
                isMoving = false;
            }
        }
    }

    public void NextPage()
    {
        if (currentPage >= totalPages - 1 || isMoving)
            return;

        currentPage++;
        MoveToPage();
    }

    public void PreviousPage()
    {
        if (currentPage <= 0 || isMoving)
            return;

        currentPage--;
        MoveToPage();
    }

    private void MoveToPage()
    {
        float newX = -currentPage * pageWidth;
        targetPosition = new Vector2(newX, 0);
        isMoving = true;
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        backButton.SetActive(currentPage > 0);

        bool isLastPage = currentPage == totalPages - 1;
        nextButton.SetActive(!isLastPage);
        startButton.SetActive(isLastPage);
    }

    public void StartGame()
    {
        // ✅ Mark onboarding as seen for this session
        onboardingSeenThisSession = true;

        StartCoroutine(FadeOutOnboarding());
    }

    private IEnumerator FadeOutOnboarding()
    {
        float alpha = 0f;

        // Fade to black
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            SetFadeAlpha(alpha);
            yield return null;
        }

        // Switch UI
        onboardingPanel.SetActive(false);
        buttonsRow.SetActive(true);

        yield return null;

        // Fade back to transparent
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            SetFadeAlpha(alpha);
            yield return null;
        }

        SetFadeAlpha(0f);
    }

    private void SetFadeAlpha(float alpha)
    {
        if (onboardingFadeOverlay == null) return;

        Color c = onboardingFadeOverlay.color;
        c.a = Mathf.Clamp01(alpha);
        onboardingFadeOverlay.color = c;
    }
}