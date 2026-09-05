using UnityEngine;

public class AimingSystem : MonoBehaviour
{
    public float reflectLength;
    public LayerMask wallLayer;

    void Update()
    {
        if (UIManager.instance.IsPaused) return;

        Vector3 touchPosition = InputManager.instance.GetLastTouchPosition(out bool touchEnded);

        if (InputManager.instance.touchStarted && !BallManager.instance.hasBeenShot && InputManager.instance.touchPaused)
        {   
            DrawLine(touchPosition);
        }
    }

    void DrawLine(Vector3 touchPosition)
    {
        Vector3 start = new Vector3(transform.position.x, transform.position.y);
        Vector3 direction = (touchPosition - start).normalized;
        RaycastHit2D hit = Physics2D.Raycast(start, direction, Mathf.Infinity, wallLayer);

        Vector2 reflectPosition = Vector2.Reflect(new Vector3(hit.point.x, hit.point.y) - start, hit.normal);
        Vector3 endPoint;

        if (hit.collider != null)
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint = start + direction * 100f;
        }

        DotsRenderer.instance.DrawDottedLine(start, endPoint);
        DotsRenderer.instance.DrawDottedLine(endPoint, hit.point + reflectPosition.normalized * reflectLength);
    }
}
