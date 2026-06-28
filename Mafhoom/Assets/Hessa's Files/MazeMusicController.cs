using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeMusicController : MonoBehaviour
{
    private AudioSource src;

    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Scene6_soomi")
        {
            src.Stop();
        }
    }
}