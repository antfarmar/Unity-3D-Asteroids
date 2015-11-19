using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// A pool that stores objects made from prefabs.
[Serializable]
public class Pool
{
    public string key;
    public int maxCount;
    public GameObject prefab;
    public List<Poolable> list;
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
    //static Dictionary<string, Pool> pools = new Dictionary<string, Pool>();
    static List<Pool> pools = new List<Pool>();
    #endregion


    #region MonoBehaviour
    // Enforce Singleton pattern on awake.
    void Awake()
    //void OnEnable()
    {
        if(instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }
    #endregion


    public static Pool FindPool(string key)
    {
        return pools.Find(x => x.key == key);
    }


    #region Public
    // Set an upper-limit on pool growth.
    public static void SetMaxCount(string key, int maxCount)
    {
        Pool pool = FindPool(key);
        if(pool == null)
            return;
        pool.key = key;
        pool.maxCount = maxCount;
    }


    // Creates a new pool for use by a prefab type & fills it with startCount new prefab instances.
    public static bool CreatePool(string key, GameObject prefab, int startCount, int maxCount)
    {
        if(FindPool(key) != null)
            return false;

        Pool pool = new Pool();
        pool.key = key;
        pool.prefab = prefab;
        pool.maxCount = maxCount;
        pool.list = new List<Poolable>(startCount);
        pools.Add(pool);

        for(int i = 0; i < startCount; ++i)
            Enqueue(CreatePoolableObject(key, prefab));

        return true;
    }


    // Removes an entire pool from use and destroys all of its objects.
    public static void DestroyPool(string key)
    {
        Pool pool = FindPool(key);
        if(pool == null)
            return;

        while(pool.list.Count > 0)
        {
            Poolable obj = pool.list[0];
            pool.list.RemoveAt(0);
            GameObject.Destroy(obj.gameObject);
        }
        pools.Remove(pool);
    }


    // Pool and deactivate an item for reuse, or destroy it if pool is full.
    public static void Enqueue(Poolable item)
    {
        Pool pool = FindPool(item.key);
        if(item == null || item.isPooled || pool == null)
            return;


        // Pool is full! Destroy the object.
        if(pool.list.Count >= pool.maxCount)
        {
            GameObject.Destroy(item.gameObject);
            return;
        }

        // Pool the object for reuse.
        pool.list.Add(item);
        item.isPooled = true;
        item.transform.SetParent(Instance.transform);
        item.gameObject.SetActive(false);
    }


    // Always returns an inactive prefab instance for the client.
    public static Poolable Dequeue(string key)
    {
        Pool pool = FindPool(key);
        //Debug.Log(pool);
        //Debug.Log(pool.list);
        if(pool == null)
            return null;

        // Pool is spent! Create a new object for client.
        if(pool.list.Count == 0)
        {
            return CreatePoolableObject(key, pool.prefab);
        }

        // Dequeue an existing object for client.
        Poolable obj = pool.list[0];
        pool.list.RemoveAt(0);
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