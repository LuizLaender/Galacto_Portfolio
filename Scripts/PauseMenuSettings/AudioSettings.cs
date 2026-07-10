using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public GameObject SettingsGroup;
    public GameObject AudioGroup;

    public void SetSFXVolume(float volume)
    {
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource obj in allAudioSources)
        {
            if (obj.CompareTag("SoundtrackPlayer")) continue;

            AudioSource source = obj.GetComponent<AudioSource>();
            if (source != null)
                source.volume = volume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        GameObject[] musicSources = GameObject.FindGameObjectsWithTag("SoundtrackPlayer");
        foreach (GameObject obj in musicSources)
        {
            AudioSource source = obj.GetComponent<AudioSource>();
            if (source != null)
                source.volume = volume;
        }
    }

    public void ShowAudioSettings()
    {
        SettingsGroup.SetActive(false);
        AudioGroup.SetActive(true);
    }

    public void HideAudioSettings()
    {
        AudioGroup.SetActive(false);
        SettingsGroup.SetActive(true);
    }
}
