using System.Collections;
using UnityEngine;

public class BallIncreasement : MonoBehaviour, IPoolable
{
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float shrinkSpeed = 1f;


    [SerializeField] private float scaleAmount = 0.2f;
    [SerializeField] private float scaleSpeed = 2f;
    private float scale;

    bool isFalling = false;
    Vector3 originalScale;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isFalling) return;

        if (collider.gameObject.CompareTag("Ball"))
        {
            BallManager.instance.IncreaseBallNumber();
            AudioManager.instance.Play(SoundType.BallCollect);
            isFalling = true;
        }

        return;
    }

    void Update()
    {
        if (!isFalling)
        {
            scale = 1f + scaleAmount * Mathf.PingPong(Time.unscaledTime * scaleSpeed, 1f);
            transform.localScale = originalScale * scale;
            return;
        }

        transform.position += Vector3.down * fallSpeed * Time.unscaledDeltaTime;

        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, shrinkSpeed * Time.unscaledDeltaTime);

        if (transform.localScale == Vector3.zero)
        {
            ObjectPoolManager.instance.Release("Ball Increasement", gameObject);
        }
    }

    public void OnSpawn()
    {
        isFalling = false;
        originalScale = transform.localScale;
    }

    public void OnDeSpawn()
    {

    }
}