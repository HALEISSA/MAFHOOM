using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [Header("Door State")]
    public bool isUnlocked = false;

    [Header("Scene")]
    public string puzzleSceneName = "Scene6_soomi";

    [Header("Locked Door Message")]
    public GameObject lockedMessagePanel;
    public TextMeshProUGUI lockedMessageText;
    public float messageDuration = 2f;

    private Coroutine messageRoutine;

    private void Start()
    {
        if (lockedMessagePanel != null)
            lockedMessagePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (isUnlocked)
        {
            Debug.Log("Door unlocked. Loading puzzle scene...");
            SceneManager.LoadScene(puzzleSceneName);
        }
        else
        {
            Debug.Log("Door is locked. Find the button first.");
            ShowLockedMessage();
        }
    }

    public void UnlockDoor()
    {
        isUnlocked = true;
        Debug.Log("Door unlocked!");
    }

    private void ShowLockedMessage()
    {
        if (lockedMessagePanel == null || lockedMessageText == null)
            return;

        if (messageRoutine != null)
            StopCoroutine(messageRoutine);

        messageRoutine = StartCoroutine(ShowMessageRoutine());
    }

    private IEnumerator ShowMessageRoutine()
    {
        lockedMessagePanel.SetActive(true);
        lockedMessageText.text = "Find the button first to unlock the door!";

        yield return new WaitForSeconds(messageDuration);

        lockedMessagePanel.SetActive(false);
    }
}