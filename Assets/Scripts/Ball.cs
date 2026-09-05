using System;
using UnityEngine;

public class Ball : MonoBehaviour, IPoolable
{
    public static Action<Ball> OnCreatedBall;
    public static Action<Ball> OnDestroyedBall;
    public static Action<Vector2> OnFirstHitDownWall;

    Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        OnCreatedBall?.Invoke(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DownWall"))
        {
            Vector2 hitPoint = collision.contacts[0].point;
            OnFirstHitDownWall?.Invoke(hitPoint);
            ObjectPoolManager.instance.Release("Ball", gameObject);
            OnDestroyedBall?.Invoke(this);
        }
        else if (!collision.gameObject.CompareTag("Brick") && !collision.gameObject.CompareTag("Ball Increasement"))
        {
            AudioManager.instance.Play(SoundType.WallHit);
        }

        AddNoise();
    }

    void AddNoise()
    {
        float speed = rigidBody.linearVelocity.magnitude;
        Vector2 direction = rigidBody.linearVelocity.normalized;

        if (Mathf.Abs(rigidBody.linearVelocity.y) < 0.05f)
        {
            direction -= UnityEngine.Random.insideUnitCircle * 0.6f;
        }

        rigidBody.linearVelocity = direction.normalized * speed;
    }

    public void OnSpawn()
    {
        AudioManager.instance.Play(SoundType.Shoot);
        rigidBody.angularVelocity = 0f;
        rigidBody.linearVelocity = Vector2.zero;
    }

    public void OnDeSpawn()
    {

    }
}
