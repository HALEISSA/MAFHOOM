using UnityEngine;
using TMPro;
using System.Collections;

public class MazeUIManager : MonoBehaviour
{
    public static MazeUIManager Instance;

    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;

    public float displayTime = 3f;

    private void Awake()
    {
        Instance = this;

        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        notificationPanel.SetActive(true);
        notificationText.text = message;

        yield return new WaitForSeconds(displayTime);

        notificationPanel.SetActive(false);
    }
}