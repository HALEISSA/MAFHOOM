using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructorSessionController : MonoBehaviourPunCallbacks
{
    [Header("Lecture Term Input")]
    public TMP_InputField termInputField;

    [Header("Terms UI")]
    public Transform termsContainer;
    public GameObject termItemPrefab;
    public TextMeshProUGUI noTermsText;

    [Header("Start Session UI")]
    public Button startLiveSessionButton;

    [Header("Persistent Notification UI")]
    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;
    public Image notificationIcon;
    public Image notificationBackground;
    public Outline notificationOutline;

    [Header("Student Progress UI")]
    public TextMeshProUGUI waitingText;
    public TextMeshProUGUI joinedStudentsText;

    [Header("Session Data")]
    public List<string> lectureTerms = new List<string>();

    private string generatedCode = "";

    private void Start()
    {
        if (notificationPanel != null)
            notificationPanel.SetActive(false);

        if (startLiveSessionButton != null)
        {
            startLiveSessionButton.onClick.RemoveAllListeners();
            startLiveSessionButton.onClick.AddListener(StartLiveSession);
        }

        UpdateNoTermsText();
        UpdateStartButtonState();
        UpdateStudentsProgressUI();
    }

    public void AddTerm()
    {
        if (termInputField == null)
        {
            Debug.LogWarning("Term input field is not assigned.");
            return;
        }

        string term = termInputField.text.Trim();

        if (string.IsNullOrEmpty(term))
        {
            Debug.LogWarning("Term is empty.");
            return;
        }

        if (lectureTerms.Contains(term))
        {
            Debug.LogWarning("Term already exists: " + term);
            termInputField.text = "";
            termInputField.ActivateInputField();
            return;
        }

        lectureTerms.Add(term);
        CreateTermItem(term);

        termInputField.text = "";
        termInputField.ActivateInputField();

        UpdateNoTermsText();
        UpdateStartButtonState();
    }

    private void CreateTermItem(string term)
    {
        GameObject item = Instantiate(termItemPrefab, termsContainer);
        TermItemUI termItemUI = item.GetComponent<TermItemUI>();

        if (termItemUI != null)
        {
            termItemUI.SetTerm(term);

            if (termItemUI.deleteButton != null)
            {
                termItemUI.deleteButton.onClick.AddListener(() =>
                {
                    lectureTerms.Remove(term);
                    Destroy(item);

                    UpdateNoTermsText();
                    UpdateStartButtonState();
                });
            }
        }
        else
        {
            Debug.LogError("TermItemPrefab is missing TermItemUI script.");
        }
    }

    private void UpdateNoTermsText()
    {
        if (noTermsText != null)
            noTermsText.gameObject.SetActive(lectureTerms.Count == 0);
    }

    private void UpdateStartButtonState()
    {
        if (startLiveSessionButton != null)
            startLiveSessionButton.interactable = lectureTerms.Count >= 2;
    }

    public void StartLiveSession()
    {
        if (lectureTerms.Count < 2)
        {
            Debug.LogWarning("You need at least 2 terms to start.");
            return;
        }

        GameManager.instance.words = new List<string>(lectureTerms);
        GameManager.instance.currentIndex = 0;
        

        generatedCode = GenerateSessionCode();

        if (PhotonLauncher.Instance == null)
        {
            Debug.LogError("PhotonLauncher instance not found.");
            return;
        }

        PhotonLauncher.Instance.CreateRoom(generatedCode, "Instructor", lectureTerms);
        ShowSessionCodeNotification(generatedCode);
    }

    private void ShowSessionCodeNotification(string code)
    {
        if (notificationPanel != null)
            notificationPanel.SetActive(true);

        if (notificationText != null)
            notificationText.text = "SESSION CODE: " + code;

        if (waitingText != null)
            waitingText.text = "WAITING FOR STUDENTS TO JOIN...";

        if (joinedStudentsText != null)
            joinedStudentsText.text = "";
    }

    private string GenerateSessionCode()
    {
        return Random.Range(100000, 999999).ToString();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateStudentsProgressUI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateStudentsProgressUI();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdateStudentsProgressUI();
    }

    private void UpdateStudentsProgressUI()
    {
        if (joinedStudentsText == null)
            return;

        if (!PhotonNetwork.InRoom)
        {
            joinedStudentsText.text = "";
            return;
        }

        List<Player> students = PhotonNetwork.PlayerList
            .Where(player => player != PhotonNetwork.LocalPlayer)
            .ToList();

        if (students.Count == 0)
        {
            joinedStudentsText.text = "";

            if (waitingText != null)
                waitingText.text = "WAITING FOR STUDENTS TO JOIN...";

            return;
        }

        List<Player> finishedStudents = students
            .Where(player =>
                player.CustomProperties.ContainsKey("progress") &&
                (int)player.CustomProperties["progress"] >= 0)
            .OrderBy(player => (int)player.CustomProperties["progress"])
            .ToList();

        List<Player> unfinishedStudents = students
            .Where(player =>
                !player.CustomProperties.ContainsKey("progress") ||
                (int)player.CustomProperties["progress"] < 0)
            .ToList();

        string progressText = "";

        foreach (Player student in unfinishedStudents)
        {
            string studentName = string.IsNullOrEmpty(student.NickName) ? "Student" : student.NickName;
            progressText += studentName + ": —\n";
        }

        int rank = 1;

        foreach (Player student in finishedStudents)
        {
            string studentName = string.IsNullOrEmpty(student.NickName) ? "Student" : student.NickName;
            int timeSpent = (int)student.CustomProperties["progress"];

            progressText += rank + ". " + studentName + " - " + timeSpent + " sec\n";
            rank++;
        }

        joinedStudentsText.text = progressText;

        if (waitingText != null)
            waitingText.text = "LIVE STUDENT PROGRESS";
    }

    public override void OnJoinedRoom()
{
    Debug.Log("Instructor joined room.");

    if (PhotonNetwork.IsMasterClient)
    {
        if (lectureTerms == null || lectureTerms.Count == 0)
        {
            Debug.LogWarning("No lecture terms to send.");
            return;
        }

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props["terms"] = string.Join("|", lectureTerms);

        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        Debug.Log("Instructor terms saved to Photon room.");
    }
}

}