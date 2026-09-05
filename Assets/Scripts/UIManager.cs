
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text score;
    public Text highScore;

    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject aboutUsPanel;
    public GameOverUI gameOverPanel;
    public WarningUI warningUI;

    public bool IsPaused { get; private set; }
    public bool SFXEnabled;
    bool musicEnabled;
    bool isGameOver;

    public Text ballCountText;
    public RectTransform ballCountRect;
    public Camera mainCamera;
    public Transform launcher;

    public static UIManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        isGameOver = false;
    }

    void Start()
    {
        UpdateScore(ScoreManager.instance.CurrentScore);
        UpdateHighScore(ScoreManager.instance.HighScore);

        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        aboutUsPanel.SetActive(false);

        SFXEnabled = true;
        musicEnabled = true;
    }

    void Update()
    {
        UpdateBallCountPosition();
    }

    void UpdateBallCountPosition()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(launcher.position);
        ballCountRect.position = screenPosition;
    }

    public void UpdateBallCount(int count)
    {
        ballCountText.text = 'X' + count.ToString();
    }

    void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScore;
        ScoreManager.OnHighScoreChanged += UpdateHighScore;
    }

    void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScore;
        ScoreManager.OnHighScoreChanged -= UpdateHighScore;
    }

    void UpdateScore(int amount)
    {
        score.text = "Score: " + amount.ToString();
    }

    void UpdateHighScore(int amount)
    {
        highScore.text = "Best: " + amount.ToString();
    }

    public void OpenPause()
    {
        IsPaused = true;

        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        InputManager.instance.touchPaused = true;
        settingsPanel.SetActive(false);
        aboutUsPanel.SetActive(false);
        pausePanel.SetActive(false);

        Time.timeScale = 1;
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenAboutUs()
    {
        aboutUsPanel.SetActive(true);
    }

    public void CloseAboutUs()
    {
        aboutUsPanel.SetActive(false);
    }

    public void ShowWarning()
    {
        if (!isGameOver)
        {
            warningUI.Show();
        }
    }

    public void ShowGameOver()
    {
        isGameOver = true;

        if (warningUI.warningText.gameObject.activeSelf)
        {
            warningUI.warningText.gameObject.SetActive(false);
        }

        gameOverPanel.Show();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;

        if (musicEnabled)
        {
            AudioManager.instance.UnPauseMusic();
        }
        else
        {
            AudioManager.instance.PauseMusic();
        }
    }

    public void ToggleSFX()
    {
        SFXEnabled = !SFXEnabled;
    }

    public void HidePanels()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        aboutUsPanel.SetActive(false);

        warningUI.ResetWarningUI();
        gameOverPanel.ResetGameOverUI();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        IsPaused = false;

        HidePanels();

        InputManager.instance.touchPaused = true;
        ScoreManager.instance.ResetScore();
        ObjectPoolManager.instance.ReleaseAllActiveObjects();
        BallManager.instance.ResetBallManager();
        BrickManager.instance.ResetBricksManager();

        isGameOver = false;
    }

    public void ResetGameAndHighScore()
    {
        RestartGame();
        InputManager.instance.touchPaused = true;
        ScoreManager.instance.ResetHighScore();
    }
}