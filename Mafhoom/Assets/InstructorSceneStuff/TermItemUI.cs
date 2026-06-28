using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TermItemUI : MonoBehaviour
{
    public TextMeshProUGUI termText;
    public Button deleteButton;

    public void SetTerm(string term)
    {
        if (termText != null)
            termText.text = term;
    }
}
