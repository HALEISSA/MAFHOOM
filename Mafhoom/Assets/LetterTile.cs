using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LetterTile : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button button;
    public Image backgroundImage;

    public string letter;
    public PuzzleController controller;

    private Color normalColor = Color.white;
    private Color selectedColor = new Color32(255, 210, 90, 255);

    private void Awake()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }

        if (backgroundImage != null)
        {
            normalColor = backgroundImage.color;
        }
    }

    public void SetLetter(string l)
    {
        letter = l;

        if (text != null)
            text.text = l;
    }

    public void SetSelected(bool selected)
    {
        if (backgroundImage != null)
            backgroundImage.color = selected ? selectedColor : normalColor;
    }

    private void OnClick()
    {
        if (controller != null)
            controller.SelectTile(this);
    }
}