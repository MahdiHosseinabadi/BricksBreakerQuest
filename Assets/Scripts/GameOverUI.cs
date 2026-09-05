using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Image background;
    public GameObject gameOverPanel;

    public float fadeSpeed = 0.2f;
    public float targetAlpha = 0.45f;

    Color color;
    bool isShowing = false;

    void Awake()
    {
        background.gameObject.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void Show()
    {
        Time.timeScale = 0;
        background.gameObject.SetActive(true);
        gameOverPanel.SetActive(false);

        color = background.color;
        color.a = 0;
        background.color = color;

        isShowing = true;
    }

    void Update()
    {
        if (!isShowing) return;

        color = background.color;
        color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.unscaledDeltaTime);

        background.color = color;

        if (Mathf.Approximately(color.a, targetAlpha))
        {
            gameOverPanel.SetActive(true);
            isShowing = false;
        }
    }

    public void ResetGameOverUI()
    {
        background.gameObject.SetActive(false);
        gameOverPanel.SetActive(false);

        color = background.color;
        color.a = 0;
        background.color = color;
    }
}
