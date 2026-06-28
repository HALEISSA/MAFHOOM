using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class InstructorProgressUI : MonoBehaviourPunCallbacks
{
    [Header("Live Progress UI")]
    public GameObject emptyState;
    public TextMeshProUGUI joinedStudentsText;

    public override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("InstructorProgressUI ENABLED");
        InvokeRepeating(nameof(RefreshStudentList), 0.5f, 0.5f);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Debug.Log("InstructorProgressUI DISABLED");
        CancelInvoke(nameof(RefreshStudentList));
    }

    private void Start()
    {
        Debug.Log("InstructorProgressUI STARTED");
        RefreshStudentList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("InstructorProgressUI detected joined student: " + newPlayer.NickName);
        RefreshStudentList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RefreshStudentList();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        RefreshStudentList();
    }

    public void RefreshStudentList()
    {
        Debug.Log("RefreshStudentList called");

        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("InstructorProgressUI: Not in room yet");
            return;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int studentCount = 0;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log("Found player: " + player.NickName + " | Master: " + player.IsMasterClient);

            if (player.IsMasterClient)
                continue;

            studentCount++;

            string name = string.IsNullOrEmpty(player.NickName) ? "Student" : player.NickName;
            string status = "Joined";
            int progress = -1;

            if (player.CustomProperties.TryGetValue("status", out object statusObj) && statusObj != null)
                status = statusObj.ToString();

            if (player.CustomProperties.TryGetValue("progress", out object progressObj) && progressObj != null)
            {
                if (progressObj is int intProgress)
                    progress = intProgress;
                else if (progressObj is float floatProgress)
                    progress = Mathf.RoundToInt(floatProgress);
                else if (progressObj is double doubleProgress)
                    progress = Mathf.RoundToInt((float)doubleProgress);
                else if (progressObj is string stringProgress)
                    int.TryParse(stringProgress, out progress);
            }

            sb.AppendLine($"▸ {name}");
sb.AppendLine($"  Status: {status}");

if (progress < 0)
    sb.AppendLine("  Progress: —");
else
    sb.AppendLine($"  Progress: {progress} sec");

sb.AppendLine();
        }

        bool hasStudents = studentCount > 0;

        if (emptyState != null)
            emptyState.SetActive(!hasStudents);
        else
            Debug.LogWarning("emptyState is NOT assigned");

        if (joinedStudentsText != null)
        {
            joinedStudentsText.gameObject.SetActive(true);
            joinedStudentsText.text = hasStudents ? sb.ToString().TrimEnd() : "";
        }
        else
        {
            Debug.LogWarning("joinedStudentsText is NOT assigned");
        }

        Debug.Log("InstructorProgressUI refreshed. Students shown: " + studentCount);
    }
}