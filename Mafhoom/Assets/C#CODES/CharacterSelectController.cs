using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI statusText;

    private bool characterChosen = false;

    public void ChooseBoy()
    {
        SelectCharacter("Boy");
    }

    public void ChooseGirl()
    {
        SelectCharacter("Girl");
    }

    private void SelectCharacter(string character)
    {
        Hashtable props = new Hashtable
        {
            { "character", character },
            { "ready", true },
            { "progress", "Ready" }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        characterChosen = true;

        if (statusText != null)
            statusText.text = "Character selected. Waiting for instructor...";
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("gameStarted"))
        {
            bool started = (bool)propertiesThatChanged["gameStarted"];

            if (started && characterChosen)
            {
                SceneManager.LoadScene("Scene_4_Gameplay");
            }
        }
    }
}
