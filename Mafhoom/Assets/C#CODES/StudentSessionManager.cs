using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class StudentSessionManager : MonoBehaviourPunCallbacks
{
    public static StudentSessionManager Instance;

    [Header("Student Info")]
    public string chosenStudentName = "Student";

    private float puzzleStartTime;
    private bool isTimingPuzzle = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetStudentName(string studentName)
    {
        chosenStudentName = studentName.Trim();

        if (string.IsNullOrEmpty(chosenStudentName))
            chosenStudentName = "Student";

        PhotonNetwork.NickName = chosenStudentName;
        Debug.Log("Student nickname set to: " + PhotonNetwork.NickName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Student joined room: " + PhotonNetwork.CurrentRoom.Name);

        SetInitialStudentProperties();

        if (RoomTermsManager.Instance != null)
            RoomTermsManager.Instance.LoadTermsFromRoom();
        else
            Debug.LogWarning("RoomTermsManager instance is missing.");
    }

    private void SetInitialStudentProperties()
    {
        Hashtable props = new Hashtable
        {
            { "status", "Joined" },
            { "progress", -1 }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Debug.Log("Initial student properties set.");
    }

    public void StartPuzzleTimer()
    {
        if (isTimingPuzzle)
            return;

        puzzleStartTime = Time.time;
        isTimingPuzzle = true;

        Hashtable props = new Hashtable
        {
            { "status", "Solving Puzzle" },
            { "progress", -1 }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        Debug.Log("Puzzle timer started.");
    }

    public void StopPuzzleTimer()
    {
        if (!isTimingPuzzle)
            return;

        isTimingPuzzle = false;

        int timeSpent = Mathf.FloorToInt(Time.time - puzzleStartTime);

        Hashtable props = new Hashtable
        {
            { "status", "Finished Puzzle" },
            { "progress", timeSpent }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        Debug.Log("Puzzle finished in: " + timeSpent + " seconds.");
    }
}