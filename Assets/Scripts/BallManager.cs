using System;
using System.Collections;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public float force;
    public GameObject prefabBall;
    public Transform launchBall;
    public bool hasBeenShot;
    public int BallNumber = 1;

    float Interval = 0.1f;
    int BallCount;
    bool waitingForFirstHit = false;
    bool shootingFinished = false;
    bool firstHitPending = false;
    bool ignoreNextBallShoot;

    Vector2 firstHitPoint;

    public static Action OnAllBallsDestroyed;

    public static BallManager instance;

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

        ignoreNextBallShoot = false;
        BallNumber = 1;
        UIManager.instance.UpdateBallCount(BallNumber);
    }

    void OnEnable()
    {
        Ball.OnCreatedBall += HandleBallCreated;
        Ball.OnDestroyedBall += HandleBallDestroyed;
        Ball.OnFirstHitDownWall += OnBallHitDownWall;
    }

    void OnDisable()
    {
        Ball.OnCreatedBall -= HandleBallCreated;
        Ball.OnDestroyedBall -= HandleBallDestroyed;
        Ball.OnFirstHitDownWall -= OnBallHitDownWall;
    }

    void HandleBallCreated(Ball ball)
    {

    }

    void HandleBallDestroyed(Ball ball)
    {
        BallCount--;

        if (BallCount <= 0)
        {
            BallCount = 0;

            if (shootingFinished && firstHitPending)
            {
                FinishFirstHit();
            }

            OnAllBallsDestroyed?.Invoke();
        }
    }

    void OnBallHitDownWall(Vector2 hitPoint)
    {
        if (!waitingForFirstHit) return;

        if (!firstHitPending)
        {
            firstHitPending = true;
            firstHitPoint = hitPoint;
        }
    }

    void Update()
    {
        hasBeenShot = (BallCount > 0);
        
        if (UIManager.instance.IsPaused) return;

        Vector3 touchPosition = InputManager.instance.GetLastTouchPosition(out bool touchEnded);

        if (ignoreNextBallShoot)
        {
            if (touchEnded)
            {
                ignoreNextBallShoot = false;
            }

            return;
        }

        if (touchEnded && !hasBeenShot)
        {
            waitingForFirstHit = true;
            shootingFinished = false;
            firstHitPending = false;
            StartCoroutine(ShootBalls(touchPosition));
        }
    }

    IEnumerator ShootBalls(Vector3 touchPosition)
    {
        for (int i = 0; i < BallNumber; i++)
        {
            Shoot(touchPosition);
            yield return new WaitForSeconds(Interval);
        }

        shootingFinished = true;

        if (firstHitPending && BallCount == 0)
        {
            FinishFirstHit();
        }
    }

    void Shoot(Vector3 touchPosition)
    {
        GameObject clone = ObjectPoolManager.instance.Get("Ball");

        if (clone != null)
        {
            clone.transform.position = launchBall.position;
            clone.transform.rotation = Quaternion.identity;

            BallCount++;

            Vector3 direction3D = (touchPosition - launchBall.position);
            Vector2 direction = new Vector2(direction3D.x, direction3D.y).normalized;

            clone.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    void FinishFirstHit()
    {
        waitingForFirstHit = false;
        firstHitPending = false;
        shootingFinished = false;

        Vector3 newPosition = launchBall.position;
        newPosition.x = firstHitPoint.x;
        launchBall.position = newPosition;

        UIManager.instance.UpdateBallCount(BallNumber);
    }

    public void PauseBallManager()
    {
        StopAllCoroutines();
        ignoreNextBallShoot = true;
    }

    public void ResetBallManager()
    {
        BallNumber = 1;
        BallCount = 0;
        hasBeenShot = false;
        waitingForFirstHit = false;
        shootingFinished = false;
        firstHitPending = false;
        firstHitPoint = new Vector2(0f, -3.8f);
        launchBall.position = firstHitPoint;
        ignoreNextBallShoot = true;

        StopAllCoroutines();

        UIManager.instance.UpdateBallCount(BallNumber);
    }
}
