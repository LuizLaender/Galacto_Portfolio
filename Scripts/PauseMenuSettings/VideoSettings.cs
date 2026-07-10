using UnityEngine;

public class VideoSettings : MonoBehaviour
{
    public GameObject SettingsGroup;
    public GameObject VideoGroup;

    public void ShowVideoSettings()
    {
        SettingsGroup.SetActive(false);
        VideoGroup.SetActive(true);
    }

    public void HideVideoSettings()
    {
        VideoGroup.SetActive(false);
        SettingsGroup.SetActive(true);
    }

    public void SetFullscreen()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }

    public void SetWindowed()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    public void SetBorderless()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    public void SetResolution_1920x1080()
    {
        Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
    }

    public void SetResolution_1280x720()
    {
        Screen.SetResolution(1280, 720, Screen.fullScreenMode);
    }

    public void SetResolution_720x480()
    {
        Screen.SetResolution(720, 480, Screen.fullScreenMode);
    }
}
