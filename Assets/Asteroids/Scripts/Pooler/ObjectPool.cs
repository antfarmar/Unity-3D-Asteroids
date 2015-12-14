using UnityEngine;
using System.Collections.Generic;
using System;

public class ObjectPool : ScriptableObject
{
    [SerializeField]
    [HideInInspector]
    GameObject prefab;

    [SerializeField]
    [HideInInspector]
    ParkingStorage parking;

    public bool IsEmpty { get { return parking.IsEmpty; } }

    public void Recycle(Poolable p)
    {
        parking.Park(p);
    }

    public T GetRecyclable<T>() where T : IRecyclable
    {
        return GetRecyclable().GetComponent<T>();
    }

    public Poolable GetRecyclable()
    {
        if (parking.IsEmpty)
            return Clone();
        else
            return (Poolable)parking.Unpark();
    }

    public static ObjectPool Build(GameObject prefab, int initialClones, int initialCapacity)
    {
        ObjectPool pool = CreateInstance<ObjectPool>();
        pool.Initialize(prefab, initialClones, initialCapacity);
        return pool;
    }

    void Initialize(GameObject prefab, int initialClones, int capacity)
    {
        this.prefab = prefab;
        parking = ParkingStorage.InfiniteSpace(capacity);
        ParkInitialClones(initialClones);
    }

    void ParkInitialClones(int initialClones)
    {
        for (int i = 0; i < initialClones; ++i)
            parking.Park(Clone());
    }

    Poolable Clone()
    {
        GameObject clone = Instantiate(prefab);
        var p = Poolable.AddPoolableComponent(clone, this);
        return p;
    }
}

//============================================================================

[Serializable]
public class ParkingStorage
{
    [SerializeField]
    [HideInInspector]
    List<Parkable> fauxStack;

    ParkingStorage()
    {
    }

    public static ParkingStorage InfiniteSpace(int capacity)
    {
        var p = new ParkingStorage();
        p.fauxStack = new List<Parkable>(capacity);
        return p;
    }

    public bool IsEmpty { get { return fauxStack.Count == 0; } }

    public void Park(Parkable p)
    {
        PushFauxStack(p);
        p.Park();
    }

    public Parkable Unpark()
    {
        Parkable p = PopFauxStack();
        p.Unpark();
        return p;
    }

    void PushFauxStack(Parkable p)
    {
        fauxStack.Add(p);
    }

    Parkable PopFauxStack()
    {
        var lastIndex = fauxStack.Count - 1;
        Parkable p = fauxStack[lastIndex];
        fauxStack.RemoveAt(lastIndex);
        return p;
    }
}