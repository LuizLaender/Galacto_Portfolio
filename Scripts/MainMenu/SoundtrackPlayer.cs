using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundtrackPlayer : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public string[] gameplayScene;

    private static SoundtrackPlayer instance;
    private AudioSource audioSource;
    private string currentScene;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.2f;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;

        if (ShouldUseAlternateMusic(currentScene))
            PlayClip(gameplayMusic);
        else
            PlayClip(menuMusic);
    }

    void PlayClip(AudioClip clip)
    {
        if (audioSource == null) return;
        if (audioSource.clip == clip) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }


    bool ShouldUseAlternateMusic(string sceneName)
    {
        foreach (string s in gameplayScene)
            if (s == sceneName)
                return true;
        return false;
    }
}
