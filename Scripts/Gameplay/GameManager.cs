using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tutorialTxt;
    [SerializeField] TextMeshProUGUI _planetsUI;
    [SerializeField] TextMeshProUGUI _starsUI;

    public GameObject tutorialUI;
    public GameObject pauseMenuUI;
    public GameObject _gameOverUI;
    private bool isPaused = false;
    public int DeathReason;
    public TextMeshProUGUI _deathReasonUI;

    public int DestroyedPlanets;
    public int DestroyedStars;

    void Start()
    {
        Application.targetFrameRate = 75;
        QualitySettings.vSyncCount = 0;
        ResumeGame();
        _gameOverUI.SetActive(false);
        StartCoroutine(MatchBegins());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void AddStarDestroyed(int amount)
    {
        DestroyedStars += amount;
    }

    public void AddPlanetDestroyed(int amount)
    {
        DestroyedPlanets += amount;
    }

    public string GetDeathReason(int deathType)
    {
        switch (deathType)
        {
            case 1:
                return "You ran out of energy!";

            case 2:
                return "The supernova killed you!";

            case 3:
                return "You were consumed by the black hole!\nSpaghetti time!";

            default:
                return "Unknown death?";
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void EnableTutorial()
    {
        tutorialUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void DisableTutorial()
    {
        tutorialUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void GameOver()
    {
        _deathReasonUI.text = GetDeathReason(DeathReason);
        _planetsUI.text = "Planets destroyed: " + DestroyedPlanets.ToString();
        _starsUI.text = "Stars destroyed: " + DestroyedStars.ToString();
        _gameOverUI.SetActive(true);
    }

    IEnumerator MatchBegins()
    {
        yield return new WaitForSeconds(8);
        _tutorialTxt.enabled = false;
    }
}
