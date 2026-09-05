using UnityEngine;

public class BrickFragment : MonoBehaviour, IPoolable
{
    private float lifeTime = 2f;
    public float minRotationSpeed = -540f;
    public float maxRotationSpeed = 540f;

    Vector3 startScale;
    Vector3 endScale = Vector3.zero;
    Vector2 randomScaleRange = new Vector2(0.4f, 1.2f);

    Rigidbody2D rigidBody;

    float currentScaleMultiplier;
    float timer;
    float rotationSpeed;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startScale = transform.localScale;
    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;
        currentScaleMultiplier = Random.Range(randomScaleRange.x, randomScaleRange.y);
        transform.localScale = startScale * currentScaleMultiplier;

        timer = lifeTime;

        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        rigidBody.angularVelocity = rotationSpeed;
    }

    void Update()
    {
        if (timer <= 0) return;

        timer -= Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(timer / lifeTime);
        transform.localScale = Vector3.Lerp(endScale, startScale * currentScaleMultiplier, t);

        if (timer <= 0)
        {
            ObjectPoolManager.instance.Release("BrickFragment", gameObject);
        }
    }

    public void OnSpawn()
    {
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
    }

    public void OnDeSpawn()
    {
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
    }
}
