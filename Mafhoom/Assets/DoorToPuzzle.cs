using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToPuzzle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player touched door"); // DEBUG
            SceneManager.LoadScene("scene6_soomi");
        }
    }
}
