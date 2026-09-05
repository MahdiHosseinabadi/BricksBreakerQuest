using UnityEngine;

public class BrickFlash : MonoBehaviour
{
    public Color flashColor;
    public float flashDuration = 0.1f;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    float timer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Flash()
    {
        spriteRenderer.color = flashColor;
        timer = flashDuration;
    }

    void Update()
    {
        if (timer < 0) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            spriteRenderer.color = originalColor;
        }
    }
}
