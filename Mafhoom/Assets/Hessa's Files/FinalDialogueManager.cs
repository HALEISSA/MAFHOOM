using UnityEngine;
using TMPro;
using System.Collections;

public class FinalDialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject darkOverlay;
    public GameObject professorPortrait;
    public TextMeshProUGUI dialogueText;

    public CanvasGroup dialogueCanvasGroup;
    public CanvasGroup professorPortraitCanvasGroup;
    public CanvasGroup darkOverlayCanvasGroup;

    public float typingSpeed = 0.04f;
    public float fadeSpeed = 4f;

    private string[] lines =
    {
        "Dr. Sofianiza: Excellent work. You made it through the challenge.",
        "Dr. Sofianiza: You did not just revise the terms — you understood them.",
        "Dr. Sofianiza: That is the real goal of MAFHOOM.",
        "Dr. Sofianiza: Now go ahead. The gate is open."
    };

    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    public System.Action OnDialogueFinished;

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (darkOverlay != null)
            darkOverlay.SetActive(false);

        if (professorPortrait != null)
            professorPortrait.SetActive(false);
    }

    public void StartDialogue()
    {
        currentLine = 0;

        dialoguePanel.SetActive(true);

        if (darkOverlay != null)
            darkOverlay.SetActive(true);

        if (professorPortrait != null)
            professorPortrait.SetActive(true);

        StartCoroutine(FadeInDialogue());
        StartTyping(lines[currentLine]);
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = lines[currentLine];
            isTyping = false;
            return;
        }

        currentLine++;

        if (currentLine < lines.Length)
        {
            StartTyping(lines[currentLine]);
        }
        else
        {
            StartCoroutine(EndDialogue());
        }
    }

    private void StartTyping(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private IEnumerator FadeInDialogue()
    {
        SetAlpha(dialogueCanvasGroup, 0f);
        SetAlpha(professorPortraitCanvasGroup, 0f);
        SetAlpha(darkOverlayCanvasGroup, 0f);

        while (GetAlpha(dialogueCanvasGroup) < 1f)
        {
            float newAlpha = GetAlpha(dialogueCanvasGroup) + Time.deltaTime * fadeSpeed;

            SetAlpha(dialogueCanvasGroup, newAlpha);
            SetAlpha(professorPortraitCanvasGroup, newAlpha);
            SetAlpha(darkOverlayCanvasGroup, Mathf.Clamp01(newAlpha * 0.7f));

            yield return null;
        }
    }

    private IEnumerator EndDialogue()
    {
        while (GetAlpha(dialogueCanvasGroup) > 0f)
        {
            float newAlpha = GetAlpha(dialogueCanvasGroup) - Time.deltaTime * fadeSpeed;

            SetAlpha(dialogueCanvasGroup, newAlpha);
            SetAlpha(professorPortraitCanvasGroup, newAlpha);
            SetAlpha(darkOverlayCanvasGroup, Mathf.Clamp01(newAlpha * 0.7f));

            yield return null;
        }

        dialoguePanel.SetActive(false);

        if (darkOverlay != null)
            darkOverlay.SetActive(false);

        if (professorPortrait != null)
            professorPortrait.SetActive(false);

        OnDialogueFinished?.Invoke();
    }

    private void SetAlpha(CanvasGroup group, float alpha)
    {
        if (group != null)
            group.alpha = Mathf.Clamp01(alpha);
    }

    private float GetAlpha(CanvasGroup group)
    {
        if (group != null)
            return group.alpha;

        return 1f;
    }
}