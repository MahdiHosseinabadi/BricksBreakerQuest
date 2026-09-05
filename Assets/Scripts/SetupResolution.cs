using UnityEngine;

public class SetupResolution : MonoBehaviour
{
    [SerializeField] private Vector2 targetAspectRatio;
    Camera targetCamera;
    Rect rect;

    void Start()
    {
        SetupAspectRatio();
    }

    void SetupAspectRatio()
    {
        float aspectRatio = targetAspectRatio.x / targetAspectRatio.y;
        float currentAspect = (float)Screen.width / (float)Screen.height;
        float heightScale = currentAspect / aspectRatio;

        targetCamera = Camera.main;
        rect = targetCamera.rect;

        rect.height = heightScale < 1 ? heightScale : 2;
        rect.width = heightScale < 1 ? 2 : 1 / heightScale;
        rect.x = heightScale < 1 ? 0 : (1 - (1 / heightScale)) / 2;
        rect.y = heightScale < 1 ? (1 - heightScale) / 2 : 0;

        targetCamera.rect = rect;
    }
}
