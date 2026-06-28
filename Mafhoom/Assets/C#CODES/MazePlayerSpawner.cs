using Photon.Pun;
using UnityEngine;

public class MazePlayerSpawner : MonoBehaviour
{
    [Header("Character Prefabs")]
    public GameObject heshamPrefab;
    public GameObject wadeemaPrefab;

    [Header("Spawn Point")]
    public Transform spawnPoint;

    private void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogWarning("Not in a Photon room.");
            return;
        }

        if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("character", out object characterObj))
        {
            Debug.LogWarning("No character selected.");
            return;
        }

        string selectedCharacter = characterObj.ToString();
        GameObject spawnedPlayer = null;

        if (selectedCharacter == "Hesham")
        {
            spawnedPlayer = PhotonNetwork.Instantiate(
                heshamPrefab.name,
                spawnPoint.position,
                Quaternion.identity
            );
        }
        else if (selectedCharacter == "Wadeema")
        {
            spawnedPlayer = PhotonNetwork.Instantiate(
                wadeemaPrefab.name,
                spawnPoint.position,
                Quaternion.identity
            );
        }
        else
        {
            Debug.LogWarning("Unknown character selected: " + selectedCharacter);
            return;
        }

        // Make the camera follow the spawned local player
        if (spawnedPlayer != null)
        {
            PhotonView playerPhotonView = spawnedPlayer.GetComponent<PhotonView>();

            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();

                if (cameraFollow != null)
                {
                    cameraFollow.target = spawnedPlayer.transform;
                }
                else
                {
                    Debug.LogWarning("CameraFollow script is missing from Main Camera.");
                }
            }
        }
    }
}