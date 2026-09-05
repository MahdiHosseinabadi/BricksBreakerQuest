using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    public static Action<int> OnScoreChanged;
    public static Action<int> OnHighScoreChanged;

    const string HighScoreKey = "HighScore";

    public static ScoreManager instance;

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

        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    void OnEnable()
    {
        Brick.OnBrickDestroyed += AddBrickDestroyedScore;
    }

    void OnDisable()
    {
        Brick.OnBrickDestroyed -= AddBrickDestroyedScore;
    }

    void AddBrickDestroyedScore()
    {
        CurrentScore++;
        OnScoreChanged?.Invoke(CurrentScore);

        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;

            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();

            OnHighScoreChanged?.Invoke(HighScore);
        }
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, HighScore);
        PlayerPrefs.Save();
    }

    public void ResetHighScore()
    {
        HighScore = 0;

        PlayerPrefs.SetInt(HighScoreKey, 0);
        PlayerPrefs.Save();

        OnHighScoreChanged?.Invoke(HighScore);
    }
}
