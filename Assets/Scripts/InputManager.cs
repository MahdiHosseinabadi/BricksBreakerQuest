using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputManager : MonoBehaviour
{
    int activeTouchId = -1;

    Vector3 LastTouchWorldPosition = Vector3.zero;
    bool touchJustEnded = false;

    Camera mainCamera;

    public float minYtouch;

    public bool touchStarted;
    public bool touchPaused;

    public static InputManager instance;

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

        mainCamera = Camera.main;
        touchStarted = false;
        touchPaused = true;
    }

    void Update()
    {
        if (touchPaused)
            HandleTouchInput();
    }

    void HandleTouchInput()
    {
        touchJustEnded = false;
        foreach (var touch in Touch.activeTouches)
        {
            if (activeTouchId == -1 && touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.touchId)) continue;

                activeTouchId = touch.touchId;
            }

            if (touch.touchId == activeTouchId)
            {
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved || touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
                {
                    Vector3 touchPosition = mainCamera.ScreenToWorldPoint(touch.screenPosition);
                    touchPosition.y = Mathf.Max(minYtouch, touchPosition.y);

                    LastTouchWorldPosition = touchPosition;
                    touchStarted = true;
                }
                else
                {
                    touchStarted = false;
                }

                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended || touch.phase == UnityEngine.InputSystem.TouchPhase.Canceled)
                {
                    activeTouchId = -1;
                    touchJustEnded = true;
                }
            }
        }
    }

    public Vector3 GetLastTouchPosition(out bool touchEnded)
    {
        touchEnded = touchJustEnded;
        return LastTouchWorldPosition;
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    public void SetTouchPaused()
    {
        touchPaused = false;
    }
}
