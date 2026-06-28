using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    public static PhotonLauncher Instance;

    [Header("Session State")]
    public string CurrentSessionCode = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ✅ clean
        }
        else
        {
            Destroy(gameObject); // ✅ clean
        }
    }

    private void Start()
    {
        Connect();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Already connected.");
            return;
        }

        PhotonNetwork.AutomaticallySyncScene = false; // ✅ important for your flow
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected: " + cause);
    }

    public void CreateRoom(string roomCode, string role, List<string> lectureTerms)
{
    PhotonNetwork.NickName = role;

    RoomOptions roomOptions = new RoomOptions();
    roomOptions.MaxPlayers = 20;

    string termsString = string.Join("|", lectureTerms);

    roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
    {
        { "terms", termsString }
    };

    roomOptions.CustomRoomPropertiesForLobby = new string[] { "terms" };

    PhotonNetwork.CreateRoom(roomCode, roomOptions);

    Debug.Log("Room created with terms: " + termsString);
}

    public void JoinRoom(string sessionCode, string playerName)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Photon is not ready yet.");
            return;
        }

        PhotonNetwork.NickName = playerName;
        CurrentSessionCode = sessionCode;

        Debug.Log("Joining room with code: " + sessionCode);
        PhotonNetwork.JoinRoom(sessionCode);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created successfully: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room successfully: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Players in room: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Create room failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Join room failed: " + message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player joined: " + newPlayer.NickName);
        Debug.Log("Players in room now: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player left: " + otherPlayer.NickName);
        Debug.Log("Players in room now: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
}