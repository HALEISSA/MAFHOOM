using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHome : MonoBehaviour
{
    public void GoHome()
    {
        SceneManager.LoadScene("Scene_1_Home");
    }
}