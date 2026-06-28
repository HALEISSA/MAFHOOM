using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHomeButton : MonoBehaviour
{
    public void GoBackHome()
    {
        SceneManager.LoadScene("Scene_1_Home");
    }
}