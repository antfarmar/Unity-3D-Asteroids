using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// A pool that stores objects made from prefabs.
public class Pool
{
    public GameObject prefab;
    public int maxCount;
    public Queue<Poolable> queue;
}


// Singleton Pattern.
public class ObjectPooler : MonoBehaviour
{

    #region Fields / Properties
    static ObjectPooler Instance
    {
        get
        {
            if(instance == null)
                CreateSharedInstance();
            return instance;
        }
    }

    // The ObjectPooler shared instance.
    static ObjectPooler instance;

    // The table of pools available for use. Each accessible by a key string.
    static Dictionary<string, Pool> pools = new Dictionary<string, Pool>();
    #endregion


    #region MonoBehaviour
    // Enforce Singleton pattern on awake.
    void Awake()
    {
        if(instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }
    #endregion


    #region Public
    // Set an upper-limit on pool growth.
    public static void SetMaxCount(string key, int maxCount)
    {
        if(!pools.ContainsKey(key))
            return;
        Pool pool = pools[key];
        pool.maxCount = maxCount;
    }


    // Creates a new pool for use by a prefab type & fills it with startCount new prefab instances.
    public static bool CreatePool(string key, GameObject prefab, int startCount, int maxCount)
    {
        if(pools.ContainsKey(key))
            return false;

        Pool pool = new Pool();
        pool.prefab = prefab;
        pool.maxCount = maxCount;
        pool.queue = new Queue<Poolable>(startCount);
        pools.Add(key, pool);

        for(int i = 0; i < startCount; ++i)
            Enqueue(CreatePoolableObject(key, prefab));

        return true;
    }


    // Removes an entire pool from use and destroys all of its objects.
    public static void DestroyPool(string key)
    {
        if(!pools.ContainsKey(key))
            return;

        Pool pool = pools[key];
        while(pool.queue.Count > 0)
        {
            Poolable obj = pool.queue.Dequeue();
            GameObject.Destroy(obj.gameObject);
        }
        pools.Remove(key);
    }


    // Pool and deactivate an item for reuse, or destroy it if pool is full.
    public static void Enqueue(Poolable item)
    {
        if(item == null || item.isPooled || !pools.ContainsKey(item.key))
            return;

        Pool pool = pools[item.key];

        // Pool is full! Destroy the object.
        if(pool.queue.Count >= pool.maxCount)
        {
            GameObject.Destroy(item.gameObject);
            return;
        }

        // Pool the object for reuse.
        pool.queue.Enqueue(item);
        item.isPooled = true;
        item.transform.SetParent(Instance.transform);
        item.gameObject.SetActive(false);
    }


    // Always returns an inactive prefab instance for the client.
    public static Poolable Dequeue(string key)
    {
        if(!pools.ContainsKey(key))
            return null;

        Pool pool = pools[key];

        // Pool is spent! Create a new object for client.
        if(pool.queue.Count == 0)
        {
            return CreatePoolableObject(key, pool.prefab);
        }

        // Dequeue an existing object for client.
        Poolable obj = pool.queue.Dequeue();
        obj.isPooled = false;
        return obj;
    }
    #endregion


    #region Private
    // Create the statically shared, persistent ObjectPooler.
    static void CreateSharedInstance()
    {
        GameObject objectPooler = new GameObject("ObjectPooler");
        DontDestroyOnLoad(objectPooler);
        instance = objectPooler.AddComponent<ObjectPooler>();
    }


    // Instantiate prefab, make it poolable, set key for dictionary.
    static Poolable CreatePoolableObject(string key, GameObject prefab)
    {
        GameObject go = Instantiate(prefab);
        Poolable p = go.AddComponent<Poolable>();
        p.key = key;
        return p;
    }
    #endregion
}