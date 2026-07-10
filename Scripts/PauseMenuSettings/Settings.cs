using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject PauseMenuButtons;
    public GameObject SettingsButtons;

    public void ShowSettings()
    {
        PauseMenuButtons.SetActive(false);
        SettingsButtons.SetActive(true);
    }

    public void HideSettings()
    {
        PauseMenuButtons.SetActive(true);
        SettingsButtons.SetActive(false);
    }
}
