using UnityEngine;
using Photon.Pun;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject heshamPrefab;
    public GameObject wadeemaPrefab;

    public Transform spawnPoint;
    public Transform walkToProfessorPoint;

    public FinalSceneManager finalSceneManager;

    void Start()
    {
        string character = "";

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("character"))
        {
            character = (string)PhotonNetwork.LocalPlayer.CustomProperties["character"];
        }
        else
        {
            Debug.LogError("No character selected in Photon Custom Properties.");
            return;
        }

        GameObject player = null;

        if (character == "Hesham")
        {
            player = Instantiate(heshamPrefab, spawnPoint.position, Quaternion.identity);
        }
        else if (character == "Wadeema")
        {
            player = Instantiate(wadeemaPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Unknown character selected: " + character);
            return;
        }

        // ✅ ADD THIS LINE HERE
        player.transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        AutoWalkToPoint autoWalk = player.AddComponent<AutoWalkToPoint>();

        if (finalSceneManager != null)
        {
            finalSceneManager.RegisterPlayer(player, autoWalk);
        }
        else
        {
            Debug.LogError("FinalSceneManager is not assigned in CharacterSpawner.");
        }

        autoWalk.StartWalking(walkToProfessorPoint);
    }
}