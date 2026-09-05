using UnityEngine;

public class BallIncreasement : MonoBehaviour, IPoolable
{
    public float fallSpeed = 5f;
    public float shrinkSpeed = 1f;
    bool isFalling = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isFalling)
        {
            if (collider.gameObject.tag == "Ball")
            {
                BallManager.instance.BallNumber++;
                AudioManager.instance.Play(SoundType.BallCollect);
                isFalling = true;
            }

            return;
        }
    }

    void Update()
    {
        if (!isFalling) return;

        transform.position += Vector3.down * fallSpeed * Time.unscaledDeltaTime;
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, shrinkSpeed * Time.unscaledDeltaTime);

        if (isFalling && transform.localScale == Vector3.zero)
        {
            ObjectPoolManager.instance.Release("Ball Increasement", gameObject);
        }
    }

    public void OnSpawn()
    {
        isFalling = false;
    }

    public void OnDeSpawn()
    {

    }
}
