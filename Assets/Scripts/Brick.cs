using UnityEngine;

public class Brick : MonoBehaviour, IPoolable
{
    public int maxHealth;
    public int health = 1;
    public int fragmentCount;
    TextMesh UIHealth;
    BrickFlash brickFlash;

    public static System.Action OnBrickDestroyed;

    void Awake()
    {
        UIHealth = GetComponentInChildren<TextMesh>();
        brickFlash = GetComponent<BrickFlash>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (health > 1)
            {
                health--;
                UpdateHealth();
                brickFlash.Flash();
                AudioManager.instance.Play(SoundType.BrickHit);
            }
            else
            {
                for (int i = 0; i < fragmentCount; i++)
                {
                    Vector3 offset = new Vector3(Random.Range(-0.5f * transform.localScale.x, 0.5f * transform.localScale.x), Random.Range(-0.5f * transform.localScale.y, 0.5f * transform.localScale.y), 0f);
                    GameObject fragment = ObjectPoolManager.instance.Get("BrickFragment");
                    fragment.GetComponent<BrickFragment>().Spawn(transform.position + offset);
                }

                AudioManager.instance.Play(SoundType.BrickDestroy);
                OnBrickDestroyed?.Invoke();
                ObjectPoolManager.instance.Release("Brick", gameObject);
            }
        }
    }

    void UpdateHealth()
    {
        UIHealth.text = health.ToString();
    }

    public void OnSpawn()
    {
        health = maxHealth;
        UpdateHealth();
    }

    public void OnDeSpawn()
    {

    }
}
