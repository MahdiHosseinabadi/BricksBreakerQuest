using UnityEngine;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    public Text warningText;

    public float lifeTime = 5f;
    public float pulseSpeed = 8f;
    public float pulseAmount = 0.2f;
    public float fadeSpeed = 0.4f;

    float timer;
    float scale;
    Vector3 originalScale;
    Color originalColor;
    Color color;

    void Awake()
    {
        originalScale = warningText.rectTransform.localScale;
        originalColor = warningText.color;
    }

    public void Show()
    {
        timer = lifeTime;

        warningText.gameObject.SetActive(true);

        warningText.rectTransform.localScale = originalScale;
        warningText.color = originalColor;
    }

    void Update()
    {
        if (timer <= 0) return;

        timer -= Time.deltaTime;

        scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        warningText.rectTransform.localScale = originalScale * scale;

        color = warningText.color;
        color.a = Mathf.MoveTowards(color.a, 0, fadeSpeed * Time.deltaTime);
        warningText.color = color;

        if (timer <= 0)
        {
            warningText.gameObject.SetActive(false);
            warningText.rectTransform.localScale = originalScale;
            warningText.color = originalColor;
        }
    }

    public void ResetWarningUI()
    {
        timer = 0;

        warningText.gameObject.SetActive(false);
        warningText.rectTransform.localScale = originalScale;
        warningText.color = originalColor;
    }
}
