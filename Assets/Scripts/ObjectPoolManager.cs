using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IPoolable
{
    public void OnSpawn();
    public void OnDeSpawn();
}

public class ObjectPoolManager : MonoBehaviour
{
    public List<PoolConfig> poolConfigs = new List<PoolConfig>();
    Dictionary<string, ObjectPool<GameObject>> pools = new Dictionary<string, ObjectPool<GameObject>>();
    Dictionary<string, HashSet<GameObject>> activeObjects = new Dictionary<string, HashSet<GameObject>>();

    public static ObjectPoolManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (PoolConfig config in poolConfigs)
            {
                CreatePool(config);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void CreatePool(PoolConfig config)
    {
        if (pools.ContainsKey(config.tag))
        {
            Debug.LogWarning($"Pool Whit tag'{config.tag}' already exist!");
            return;
        }

        activeObjects.Add(config.tag, new HashSet<GameObject>());

        var pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(config.Prefab,transform),
            actionOnGet: obj =>
            {
                obj.SetActive(true);
                activeObjects[config.tag].Add(obj);

                var poolables = obj.GetComponentsInChildren<IPoolable>(true);
                foreach (var p in poolables) p.OnSpawn();
            },
            actionOnRelease: obj =>
            {
                var poolables = obj.GetComponentsInChildren<IPoolable>(true);
                foreach (var p in poolables) p.OnDeSpawn();

                activeObjects[config.tag].Remove(obj);
                obj.SetActive(false);
            },
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: config.defaultCapacity,
            maxSize: config.maxCapacity
        );

        pools.Add(config.tag, pool);

        if (config.prewarm)
        {
            PreWarmPool(pools[config.tag], config.defaultCapacity);
        }
    }

    void PreWarmPool(ObjectPool<GameObject> pool, int count)
    {
        GameObject[] TempArray = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            TempArray[i] = pool.Get();
        }

        for (int i = 0; i < count; i++)
        {
            pool.Release(TempArray[i]);
        }
    }

    public GameObject Get(string tag)
    {
        if (!pools.ContainsKey(tag))
        {
            Debug.LogError($"Pool Whit tag '{tag}' not found!");
            return null;
        }

        return pools[tag].Get();
    }

    public void Release(string tag, GameObject obj)
    {
        if (obj == null) return;

        if (!pools.ContainsKey(tag))
        {
            Debug.LogError($"Pool Whit tag '{tag}' not found! Destroying object.");
            Destroy(obj);
            return;
        }

        pools[tag].Release(obj);
    }

    public void ReleaseAllActiveObjects()
    {
        foreach (var item in activeObjects)
        {
            string key = item.Key;

            GameObject[] objects = new GameObject[item.Value.Count];
            item.Value.CopyTo(objects);

            foreach (GameObject obj in objects)
            {
                Release(key, obj);
            }
        }
    }
}

[System.Serializable]
public class PoolConfig
{
    public string tag;
    public GameObject Prefab;
    public int defaultCapacity;
    public int maxCapacity;
    public bool prewarm;
}
