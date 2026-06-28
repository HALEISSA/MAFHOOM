using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RoomTermsManager : MonoBehaviour
{
    public static RoomTermsManager Instance;

    public List<string> roomTerms = new List<string>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadTermsFromRoom();
    }

    public void LoadTermsFromRoom()
    {
        roomTerms.Clear();

        if (!PhotonNetwork.InRoom)
        {
            Debug.LogWarning("Not in Photon room.");
            return;
        }

        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("terms"))
        {
            Debug.LogWarning("No terms found in room.");
            return;
        }

        string termsString = PhotonNetwork.CurrentRoom.CustomProperties["terms"].ToString();

        if (string.IsNullOrEmpty(termsString))
        {
            Debug.LogWarning("Terms string is empty.");
            return;
        }

        roomTerms.AddRange(termsString.Split('|'));

        Debug.Log("Loaded terms: " + string.Join(", ", roomTerms));
    }

    // ⭐ THIS IS THE MISSING FUNCTION
    public string GetRandomTerm()
    {
        if (roomTerms == null || roomTerms.Count == 0)
        {
            Debug.LogWarning("No terms available.");
            return "";
        }

        int index = Random.Range(0, roomTerms.Count);
        return roomTerms[index];
    }
}