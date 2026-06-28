using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviourPunCallbacks
{
    [Header("Cards")]
    public Image heshamCard;
    public Image wadeemaCard;

    [Header("Checkmarks")]
    public GameObject heshamCheck;
    public GameObject wadeemaCheck;

    [Header("Start Button")]
    public Button startButton;
    public Image startButtonImage;

    [Header("Status Text")]
    public TextMeshProUGUI statusText;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip heshamVoice;
    public AudioClip wadeemaVoice;

    [Header("Start Button Sprites")]
    public Sprite startDisabledSprite;
    public Sprite startEnabledSprite;

    private string selectedCharacter = "";

    private Color normalColor = new Color32(36, 37, 41, 255);
    private Color highlightPink = new Color32(71, 43, 63, 255);
    private Color highlightBlue = new Color32(43, 55, 82, 255);

    private void Start()
    {
        heshamCheck.SetActive(false);
        wadeemaCheck.SetActive(false);

        if (heshamCard != null) heshamCard.color = normalColor;
        if (wadeemaCard != null) wadeemaCard.color = normalColor;

        startButton.interactable = false;

        if (startButtonImage != null && startDisabledSprite != null)
            startButtonImage.sprite = startDisabledSprite;

        if (statusText != null)
            statusText.text = "Choose your learning companion.";
    }

    public void SelectHesham()
    {
        Debug.Log("Hesham clicked");

        selectedCharacter = "Hesham";

        if (heshamCard != null) heshamCard.color = highlightBlue;
        if (wadeemaCard != null) wadeemaCard.color = normalColor;

        heshamCheck.SetActive(true);
        wadeemaCheck.SetActive(false);

        PlayVoice(heshamVoice);

        EnableStart("Hesham is ready to begin.");
    }

    public void SelectWadeema()
    {
        Debug.Log("Wadeema clicked");

        selectedCharacter = "Wadeema";

        if (wadeemaCard != null) wadeemaCard.color = highlightPink;
        if (heshamCard != null) heshamCard.color = normalColor;

        wadeemaCheck.SetActive(true);
        heshamCheck.SetActive(false);

        PlayVoice(wadeemaVoice);

        EnableStart("Wadeema is ready to begin.");
    }

    private void PlayVoice(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.Stop();
        audioSource.PlayOneShot(clip);
    }

    private void EnableStart(string message)
    {
        startButton.interactable = true;

        if (startButtonImage != null && startEnabledSprite != null)
            startButtonImage.sprite = startEnabledSprite;

        if (statusText != null)
            statusText.text = message;
    }

    public void StartSession()
{
    if (string.IsNullOrEmpty(selectedCharacter))
        return;

    Hashtable props = new Hashtable
    {
        { "character", selectedCharacter },
        { "ready", true },
        { "progress", "Loading" }
    };

    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

    if (statusText != null)
        statusText.text = "Preparing your learning journey...";

    PhotonNetwork.LoadLevel("Scene_4_5_Loading");
}

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("gameStarted"))
        {
            bool started = (bool)propertiesThatChanged["gameStarted"];

            if (started && !string.IsNullOrEmpty(selectedCharacter))
            {
                SceneManager.LoadScene("Scene_5_SoomiMaze");
            }
        }
    }
}