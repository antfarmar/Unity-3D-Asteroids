using System;
using UnityEngine;

public class GameBehaviour : MonoBehaviour, PoolableAware, Recyclable
{
    Poolable poolable;

    void PoolableAware.PoolableAwoke(Poolable p)
    {
        poolable = p;
    }

    public void RemoveFromGame()
    {
        if (poolable)
            poolable.Recycle();
        else
            RequestDestruction();
    }

    protected virtual void RequestDestruction()
    {
        RequestDefaultDestruction(gameObject);
    }

    public void InvokeRemoveFromGame(float time)
    {
        Invoke("RemoveFromGame", time);
    }

    public static void KillWithExplosion(GameObject victim)
    {
        Spawn.Explosion(victim.transform.position);
        RemoveFromGame(victim);
    }

    public static void RemoveFromGame(GameObject victim)
    {
        GameBehaviour handler = victim.GetComponent<GameBehaviour>();
        if (handler)
            handler.RemoveFromGame();
        else
            RequestDefaultDestruction(victim);
    }

    protected void Score(int score)
    {
        global::Score.Earn(score);
    }

    static void RequestDefaultDestruction(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    void Recyclable.Recycle()
    {
        RemoveFromGame();
    }
}
