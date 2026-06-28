using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public void SelectBoy()
    {
      PlayerPrefs.SetInt("SelectedCharacter", 0);
    Debug.Log("Boy Selected");
    SceneManager.LoadScene("Scene4_Soomi");
    }

    public void SelectGirl()
    {
        PlayerPrefs.SetInt("SelectedCharacter", 1);
        SceneManager.LoadScene("Scene4_Soomi"); // same here
    }
}

