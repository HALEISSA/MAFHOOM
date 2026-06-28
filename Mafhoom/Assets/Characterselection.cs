using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerSelector : MonoBehaviour
{
        public void SelectBoy()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 0);
        SceneManager.LoadScene("Scene4_soomi");
    }

    public void SelectGirl()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 1);
        SceneManager.LoadScene("Scene4_soomi");
    }
}

