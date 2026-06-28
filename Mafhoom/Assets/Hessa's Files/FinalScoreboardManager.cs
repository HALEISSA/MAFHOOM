using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class FinalScoreboardManager : MonoBehaviour
{
    public GameObject scoreboardPanel;
    public TextMeshProUGUI scoreboardText;

    public void ShowScoreboard()
    {
        scoreboardPanel.SetActive(true);

        string result = "";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int score = 0;

            if (player.CustomProperties.ContainsKey("score"))
            {
                score = (int)player.CustomProperties["score"];
            }

            result += player.NickName + "  -  " + score + " pts\n";
        }

        if (string.IsNullOrEmpty(result))
        {
            result = "No students found.";
        }

        scoreboardText.text = result;
    }
}