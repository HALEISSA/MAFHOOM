using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructorTermsController : MonoBehaviour
{
    [Header("Input")]
    public TMP_InputField inputField;
    public Button addButton;

    [Header("Terms List")]
    public Transform termsContainer;
    public TextMeshProUGUI emptyHint;

    [Header("Start Button")]
    public Button startButton;
    public Image startButtonImage;
    public Sprite disabledSprite;
    public Sprite enabledSprite;

    [Header("Rules")]
    public int minTermsToStart = 2;
    public int maxTerms = 10;

    [Header("Term Style")]
    public TMP_FontAsset termFont;
    public TMP_FontAsset deleteFont;

    private readonly List<string> terms = new();

    private void Start()
    {
        addButton.onClick.AddListener(AddTerm);
        RefreshUI();
    }

    public void AddTerm()
    {
        string newTerm = inputField.text.Trim();

        if (string.IsNullOrEmpty(newTerm))
            return;

        if (terms.Count >= maxTerms)
            return;

        foreach (string existing in terms)
        {
            if (string.Equals(existing, newTerm, StringComparison.OrdinalIgnoreCase))
            {
                inputField.text = "";
                inputField.ActivateInputField();
                return;
            }
        }

        terms.Add(newTerm);
        CreateTermRow(newTerm);

        inputField.text = "";
        inputField.ActivateInputField();

        RefreshUI();
    }

    private void CreateTermRow(string term)
    {
        // ROOT ROW
        GameObject row = new GameObject(
            "Term_" + term,
            typeof(RectTransform),
            typeof(Image),
            typeof(LayoutElement),
            typeof(HorizontalLayoutGroup)
        );

        row.transform.SetParent(termsContainer, false);

        RectTransform rowRT = row.GetComponent<RectTransform>();
        rowRT.localScale = Vector3.one;
        rowRT.sizeDelta = new Vector2(0, 56);

        Image rowImage = row.GetComponent<Image>();
        rowImage.color = new Color32(45, 58, 88, 255);

        LayoutElement rowLayout = row.GetComponent<LayoutElement>();
        rowLayout.preferredHeight = 56;
        rowLayout.minHeight = 56;

        HorizontalLayoutGroup hlg = row.GetComponent<HorizontalLayoutGroup>();
hlg.padding = new RectOffset(16, 16, 8, 8);
hlg.spacing = 12;
hlg.childAlignment = TextAnchor.MiddleLeft;
hlg.childControlWidth = true;
hlg.childControlHeight = true;
hlg.childForceExpandWidth = false;
hlg.childForceExpandHeight = false;

        // TEXT OBJECT
        GameObject textGO = new GameObject(
            "Txt_Term",
            typeof(RectTransform),
            typeof(TextMeshProUGUI),
            typeof(LayoutElement)
        );

        textGO.transform.SetParent(row.transform, false);

        RectTransform textRT = textGO.GetComponent<RectTransform>();
        textRT.localScale = Vector3.one;

        LayoutElement textLayout = textGO.GetComponent<LayoutElement>();
        
        textLayout.flexibleWidth = 1;
        textLayout.minHeight = 40;

        TextMeshProUGUI termText = textGO.GetComponent<TextMeshProUGUI>();
        termText.font = termFont != null ? termFont : TMP_Settings.defaultFontAsset;
        termText.text = term;
        termText.fontSize = 20;
        termText.color = Color.white;
        termText.alignment = TextAlignmentOptions.MidlineLeft;
        termText.enableWordWrapping = false;
        termText.overflowMode = TextOverflowModes.Ellipsis;

        // DELETE BUTTON
        GameObject deleteGO = new GameObject(
            "Btn_Delete",
            typeof(RectTransform),
            typeof(Image),
            typeof(Button),
            typeof(LayoutElement)
        );

        deleteGO.transform.SetParent(row.transform, false);

        RectTransform deleteRT = deleteGO.GetComponent<RectTransform>();
        deleteRT.localScale = Vector3.one;
        deleteRT.sizeDelta = new Vector2(36, 36);

        LayoutElement deleteLayout = deleteGO.GetComponent<LayoutElement>();
        deleteLayout.preferredWidth = 36;
        deleteLayout.preferredHeight = 36;
        deleteLayout.minWidth = 36;
        deleteLayout.minHeight = 36;
        deleteLayout.flexibleWidth = 0;

        Image deleteImage = deleteGO.GetComponent<Image>();
        deleteImage.color = new Color32(120, 40, 40, 255);

        Button deleteButton = deleteGO.GetComponent<Button>();

        // DELETE LABEL
        GameObject deleteTextGO = new GameObject(
            "DeleteLabel",
            typeof(RectTransform),
            typeof(TextMeshProUGUI)
        );

        deleteTextGO.transform.SetParent(deleteGO.transform, false);

        RectTransform deleteTextRT = deleteTextGO.GetComponent<RectTransform>();
        deleteTextRT.anchorMin = Vector2.zero;
        deleteTextRT.anchorMax = Vector2.one;
        deleteTextRT.offsetMin = Vector2.zero;
        deleteTextRT.offsetMax = Vector2.zero;
        deleteTextRT.localScale = Vector3.one;

        TextMeshProUGUI deleteText = deleteTextGO.GetComponent<TextMeshProUGUI>();
        deleteText.font = deleteFont != null ? deleteFont : TMP_Settings.defaultFontAsset;
        deleteText.text = "X";
        deleteText.fontSize = 18;
        deleteText.color = Color.white;
        deleteText.alignment = TextAlignmentOptions.Center;

        deleteButton.onClick.AddListener(() => RemoveTerm(row, term));
    }

    private void RemoveTerm(GameObject row, string term)
    {
        terms.RemoveAll(t => string.Equals(t, term, StringComparison.OrdinalIgnoreCase));

        if (row != null)
            Destroy(row);

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (emptyHint != null)
            emptyHint.gameObject.SetActive(terms.Count == 0);

        bool canStart = terms.Count >= minTermsToStart;

        if (startButton != null)
            startButton.interactable = canStart;

        if (startButtonImage != null && disabledSprite != null && enabledSprite != null)
            startButtonImage.sprite = canStart ? enabledSprite : disabledSprite;

        bool reachedMax = terms.Count >= maxTerms;

        if (addButton != null)
            addButton.interactable = !reachedMax;

        if (inputField != null)
            inputField.interactable = !reachedMax;
    }
}