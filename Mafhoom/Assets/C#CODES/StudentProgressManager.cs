using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class StudentProgressManager : MonoBehaviour
{
    public static StudentProgressManager Instance;

    private int currentProgress = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetStatus(string newStatus)
    {
        Hashtable props = new Hashtable
        {
            { "status", newStatus },
            { "progress", currentProgress }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Debug.Log("Student status updated to: " + newStatus);
    }

    public void IncreaseProgress(string newStatus)
    {
        currentProgress++;

        Hashtable props = new Hashtable
        {
            { "status", newStatus },
            { "progress", currentProgress }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        Debug.Log("Student progress increased to: " + currentProgress);
    }

    public int GetCurrentProgress()
    {
        return currentProgress;
    }
}