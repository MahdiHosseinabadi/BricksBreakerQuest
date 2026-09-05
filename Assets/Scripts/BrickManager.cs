using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public static event System.Action OnNextRoundStarted;

    public GameObject prefabBrick;
    public GameObject prefabBooster;
    public Transform Origin;
    public Transform BricksParent;
    public Transform referencePoint;
    public int BrickCount;
    public int PossibilityOfNoEmptySpaceCreate;
    public int PossibilityOfBoosterCreate;

    int CurrentRound;
    float worldWidth;
    float brickWidth;
    Camera camera;

    public static BrickManager instance;

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
    }

    void Start()
    {
        // isResetting = false;
        CurrentRound = 1;
        camera = Camera.main;
        CalculateWorldWidth();
        CalculateBrickWidth();
        SetOriginPosition();
        CreateBrick();
    }

    void CalculateWorldWidth()
    {
        float leftEdge = camera.ViewportToWorldPoint(new Vector3(0, 0, 10)).x;
        float rightEdge = camera.ViewportToWorldPoint(new Vector3(1, 0, 10)).x;

        worldWidth = rightEdge - leftEdge;
    }

    void CalculateBrickWidth()
    {
        brickWidth = worldWidth / BrickCount;
    }

    void SetOriginPosition()
    {
        float yPosition = referencePoint.position.y - (brickWidth / 2f);
        Origin.position = new Vector3(Origin.position.x, yPosition, Origin.position.z);
    }

    void CreateBrick()
    {
        float leftEdge = camera.ViewportToWorldPoint(new Vector3(0, 0, 10)).x;
        int randomEmptySpace = 0;
        int randomBooster = 0;

        for (int i = 0; i < BrickCount; i++)
        {
            randomEmptySpace = Random.Range(0, PossibilityOfNoEmptySpaceCreate);

            if (randomEmptySpace != 1)
            {
                float xPosition = leftEdge + (i * brickWidth) + (brickWidth / 2f);
                Vector3 position = new Vector3(xPosition, Origin.position.y, 0);
                GameObject brick = ObjectPoolManager.instance.Get("Brick");

                if (brick != null)
                {
                    brick.transform.SetParent(BricksParent);
                    brick.transform.position = position;
                    brick.transform.rotation = Quaternion.identity;
                    brick.transform.localScale = new Vector3(brickWidth, brickWidth, brick.transform.localScale.z);

                    Brick brickScript = brick.GetComponent<Brick>();

                    if (brickScript != null)
                    {
                        brickScript.maxHealth = CurrentRound;
                        brickScript.OnSpawn();
                    }
                }
            }
            else
            {
                randomBooster = Random.Range(0, PossibilityOfBoosterCreate);

                if (randomBooster == 1)
                {
                    float xPosition = leftEdge + (i * brickWidth) + (brickWidth / 2f);
                    Vector3 position = new Vector3(xPosition, Origin.position.y, 0);
                    GameObject Booster = ObjectPoolManager.instance.Get("Ball Increasement");

                    if (Booster != null)
                    {
                        Booster.transform.SetParent(BricksParent);
                        Booster.transform.position = position;
                        Booster.transform.rotation = Quaternion.identity;
                        Booster.transform.localScale = new Vector3(brickWidth, brickWidth, Booster.transform.localScale.z);
                    }
                }
            }
        }
    }

    void OnEnable()
    {
        BallManager.OnAllBallsDestroyed += GoNextRound;
    }

    void OnDisable()
    {
        BallManager.OnAllBallsDestroyed -= GoNextRound;
    }

    void GoNextRound()
    {
        BricksParent.position -= new Vector3(0, brickWidth, 0);
        CurrentRound++;
        CreateBrick();

        OnNextRoundStarted?.Invoke();
        AudioManager.instance.Play(SoundType.NextRound);
    }

    public void ResetBricksManager()
    {
        CurrentRound = 1;
        CreateBrick();
    }
}
