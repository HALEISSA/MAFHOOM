using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource src;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        src = GetComponent<AudioSource>();
        src.loop = true;
        src.playOnAwake = true;
    }

    private void Start()
    {
        HandleMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleMusicForScene(scene.name);
    }

    private void HandleMusicForScene(string sceneName)
    {
        if (sceneName == "Scene_1_Home" ||
            sceneName == "Scene2_hessa" ||
            sceneName == "Scene3_hessa")
        {
            if (!src.isPlaying)
                src.Play();
        }
        else
        {
            src.Stop();
        }
    }
}