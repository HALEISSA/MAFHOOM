using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class PlayerSpawner : MonoBehaviour
{
    public GameObject boyPrefab;
    public GameObject girlPrefab;

    void Start()
    {
     int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter");
Debug.Log("SelectedCharacter = " + selectedCharacter);
        GameObject player;

        if (selectedCharacter == 0)
            player = Instantiate(boyPrefab, transform.position, Quaternion.identity);
        else
            player = Instantiate(girlPrefab, transform.position, Quaternion.identity);

        Camera.main.GetComponent<CameraFollow>().target = player.transform;
    }
}

