using UnityEngine;
using System.Collections.Generic;
using System;

// A pool that stores objects made from prefabs.
[Serializable]
public class ObjectPool
{
    public int m_MaxCount;
    public GameObject m_Prefab;
    public List<Poolable> m_Pool;

    private Transform m_Parent;


    // Constructor
    public ObjectPool(GameObject prefab, Transform parent, int startCount, int maxCount)
    {
        m_Pool = new List<Poolable>(startCount);
        m_Prefab = prefab;
        m_Parent = parent != null ? parent : new GameObject("ObjectPool").transform;
        m_MaxCount = maxCount;

        for(int i = 0; i < startCount; ++i)
            Push(CreatePoolableObject());
    }


    // Instantiate prefab, make it poolable.
    Poolable CreatePoolableObject()
    {
        Poolable obj = GameObject.Instantiate(m_Prefab).AddComponent<Poolable>();
        obj.transform.SetParent(m_Parent);
        obj.gameObject.SetActive(false);
        return obj;
    }



    // Set an upper-limit on pool growth.
    public void SetMaxCount(int maxCount)
    {
        m_MaxCount = maxCount;
    }



    // Removes an entire pool from use and destroys all of its objects.
    public void EmptyPool()
    {
        while(m_Pool.Count > 0)
        {
            GameObject obj = m_Pool[m_Pool.Count - 1].gameObject;
            GameObject.Destroy(obj.gameObject);
            m_Pool.RemoveAt(m_Pool.Count - 1);
        }
    }


    // Pool and deactivate an item for reuse, or destroy it if pool is full.
    public void Push(Poolable obj)
    {
        if(obj == null || obj.isPooled)
            return;


        // Pool is full! Destroy the object.
        if(m_Pool.Count >= m_MaxCount)
        {
            GameObject.Destroy(obj.gameObject);
            return;
        }

        // Pool the object for reuse.
        m_Pool.Add(obj);
        obj.isPooled = true;
        //obj.transform.SetParent(Instance.transform);
        obj.gameObject.SetActive(false);
    }


    // Always returns an inactive prefab instance for the client.
    public Poolable Pop()
    {
        // Pool is spent! Create a new object for client.
        if(m_Pool.Count == 0)
        {
            return CreatePoolableObject();
        }

        // Dequeue an existing object for client.
        Poolable obj = m_Pool[m_Pool.Count - 1];
        m_Pool.RemoveAt(m_Pool.Count - 1);
        obj.isPooled = false;
        return obj;
    }

}