using System.Collections.Generic;
using UnityEngine;

public class DotsRenderer : MonoBehaviour
{
    public Sprite dot;

    [Range(0.01f, 1f)]
    public float size;

    [Range(0.01f, 2f)]
    public float delta;

    [Range(0.01f, 1f)]
    public float alpha;

    List<Vector2> positions = new List<Vector2>();
    List<GameObject> dots = new List<GameObject>();

    public static DotsRenderer instance;

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

    void Update()
    {
        if (positions.Count > 0)
        {
            DestroyAllDots();
            positions.Clear();
        }
    }

    void DestroyAllDots()
    {
        foreach (var dot in dots)
        {
            Destroy(dot);
        }

        dots.Clear();
    }

    GameObject GetOneDot()
    {
        GameObject gameObject = new GameObject();

        gameObject.transform.localScale = Vector3.one * size;
        gameObject.transform.parent = transform;

        var SR = gameObject.AddComponent<SpriteRenderer>();
        SR.sprite = dot;
        SR.color = new Color(1, 1, 1, alpha);
        SR.sortingOrder = 1;

        return gameObject;
    }

    public void DrawDottedLine(Vector2 start, Vector2 end)
    {
        DestroyAllDots();

        Vector2 point = start;
        Vector2 direction = (end - start).normalized;

        while ((end - start).magnitude > (point - start).magnitude)
        {
            positions.Add(point);
            point += (direction * delta);
        }

        Renderer();
    }

    void Renderer()
    {
        foreach (var position in positions)
        {
            var Object = GetOneDot();
            Object.transform.position = position;
            dots.Add(Object);
        }
    }
}
