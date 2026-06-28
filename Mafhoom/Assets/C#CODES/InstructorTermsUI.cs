using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructorTermsUI : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private TMP_InputField termInput;
    [SerializeField] private Button addButton;

    [Header("List")]
    [SerializeField] private Transform listContent;     // ScrollRect Content
    [SerializeField] private GameObject termItemPrefab; // prefab with TMP text

    [Header("Empty Hint")]
    [SerializeField] private GameObject emptyHintObject; // "No terms added yet" text object

    [Header("Start Session Button")]
    [SerializeField] private Button startSessionButton;
    [SerializeField] private Image startSessionImage;
    [SerializeField] private Sprite startDisabledSprite; // dark green (or current)
    [SerializeField] private Sprite startEnabledSprite;  // light green
    [SerializeField] private int minTermsToStart = 2;

    [Header("Limits")]
    [SerializeField] private int maxTerms = 10;

    private readonly List<string> terms = new();

    private void Awake()
    {
        // Button clicks
        addButton.onClick.AddListener(AddTermFromInput);

        // Press Enter inside input to add
        termInput.onSubmit.AddListener(_ => AddTermFromInput());

        RefreshUI();
    }

    private void AddTermFromInput()
    {
        Debug.Log("ADD BUTTON CLICKED");
        
        if (terms.Count >= maxTerms) return;

        string raw = termInput.text;
        string term = raw.Trim();

        if (string.IsNullOrEmpty(term)) return;

        // Optional: prevent duplicates (case-insensitive)
        foreach (var t in terms)
            if (string.Equals(t, term, System.StringComparison.OrdinalIgnoreCase))
            {
                termInput.text = "";
                termInput.ActivateInputField();
                return;
            }

        terms.Add(term);
        SpawnTermItem(term);

        // Clear input and focus again
        termInput.text = "";
        termInput.ActivateInputField();

        RefreshUI();
    }

    private void SpawnTermItem(string term)
    {
        GameObject go = Instantiate(termItemPrefab, listContent);
        var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
            tmp.text = "� " + term;
    }

    private void RefreshUI()
    {
        // Empty hint
        if (emptyHintObject != null)
            emptyHintObject.SetActive(terms.Count == 0);

        // Start button enable + sprite swap
        bool canStart = terms.Count >= minTermsToStart;

        if (startSessionButton != null)
            startSessionButton.interactable = canStart;

        if (startSessionImage != null)
            startSessionImage.sprite = canStart ? startEnabledSprite : startDisabledSprite;

        // Add button/input lock if reached max
        bool atLimit = terms.Count >= maxTerms;
        addButton.interactable = !atLimit;

        // You can also disable input if limit reached
        termInput.interactable = !atLimit;
        if (atLimit)
            termInput.placeholder?.GetComponent<TextMeshProUGUI>()?.SetText($"Limit reached ({maxTerms})");
    }

    // Optional helper if you need the terms later (Photon/networking/etc.)
    public IReadOnlyList<string> GetTerms() => terms;
}
