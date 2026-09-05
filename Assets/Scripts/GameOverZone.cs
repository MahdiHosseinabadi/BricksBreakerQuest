using UnityEngine;

public enum TypeZone { Warning, Lose }

public class GameOverZone : MonoBehaviour
{
    public TypeZone typeZone;
    bool isTrigger = false;

    void OnEnable()
    {
        BrickManager.OnNextRoundStarted += ResetZone;
    }

    void OnDisable()
    {
        BrickManager.OnNextRoundStarted -= ResetZone;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTrigger) return;

        if (collision.gameObject.CompareTag("Brick"))
        {
            isTrigger = true;

            switch (typeZone)
            {
                case TypeZone.Warning:
                    TriggerWarning();
                    break;
                case TypeZone.Lose:
                    TriggerGameOver();
                    break;
            }
        }
    }

    void TriggerWarning()
    {
        UIManager.instance.ShowWarning();
    }

    void TriggerGameOver()
    {
        ScoreManager.instance.SaveHighScore();
        AudioManager.instance.Play(SoundType.GameOver);
        UIManager.instance.ShowGameOver();
    }

    public void ResetZone()
    {
        isTrigger = false;
    }
}