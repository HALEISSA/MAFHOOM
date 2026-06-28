using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToHome : MonoBehaviour
{
    public void GoHome()
    {
        SceneManager.LoadScene("Scene_1_Home");
    }
}
