using UnityEngine;
using UnityEngine.SceneManagement;

public class RoleSelectUI : MonoBehaviour
{
    [SerializeField] private string instructorScene = "Scene2_hessa";
    [SerializeField] private string studentScene = "Scene3_hessa";

    [SerializeField] private AudioSource clickSound;
    [SerializeField] private SceneFader fader;

    public void ChooseInstructor()
    {
        PlayClick();
        LoadScene(instructorScene);
    }

    public void ChooseStudent()
    {
        PlayClick();
        LoadScene(studentScene);
    }

    private void PlayClick()
    {
        if (clickSound != null)
            clickSound.Play();
    }

    private void LoadScene(string sceneName)
    {
        if (fader != null)
            fader.FadeToScene(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }
}