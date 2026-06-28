using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI messageText;

    void Awake()
    {
        Instance = this;
        messageText.gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        StartCoroutine(DisplayMessage(message));
    }

    IEnumerator DisplayMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        messageText.gameObject.SetActive(false);
    }
}
