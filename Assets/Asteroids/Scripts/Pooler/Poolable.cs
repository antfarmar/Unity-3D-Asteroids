using UnityEngine;
using System;

[Serializable]
[ExecuteInEditMode]
public sealed class Poolable : Parkable
{
    [SerializeField]
    [HideInInspector]
    ObjectPool pool;

    static bool scriptBuiltInstance;

    void Awake()
    {
        InstantiationGuard();
    }

    private void InstantiationGuard()
    {
        if (!scriptBuiltInstance)
        {
            DestroyImmediate(this, true);
            throw new InvalidOperationException("Can only be created with AddPoolableComponent");
        }
        scriptBuiltInstance = false;
    }

    void OnEnable()
    {
        gameObject.hideFlags = 0;
    }

    void OnDisable()
    {
        gameObject.hideFlags = HideFlags.HideInHierarchy;
    }

    public override void Unpark()
    {
        gameObject.SetActive(true);
    }

    public override void Park()
    {
        gameObject.SetActive(false);
    }

    public void Recycle()
    {
        pool.Recycle(this);
    }

    public static Poolable AddPoolableComponent(GameObject newInstance, ObjectPool pool)
    {
        scriptBuiltInstance = true;
        var instance = newInstance.AddComponent<Poolable>();
        instance.pool = pool;
        return instance;
    }
}

public abstract class Parkable : MonoBehaviour
{
    abstract public void Park();
    abstract public void Unpark();
}