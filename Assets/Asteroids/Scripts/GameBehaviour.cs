using System;
using UnityEngine;

public class GameBehaviour : MonoBehaviour, IPoolableAware, IRecyclable
{
    Poolable poolable;

    void IPoolableAware.PoolableAwoke(Poolable p)
    {
        poolable = p;
    }

    void IRecyclable.Recycle()
    {
        RemoveFromGame();
    }

    protected virtual void OnDisable()
    {
        CancelInvoke("RemoveFromGame");
    }

    public void RemoveFromGame()
    {
        if (poolable)
            poolable.Recycle();
        else
            RequestDestruction();
    }

    public void InvokeRemoveFromGame(float time)
    {
        Invoke("RemoveFromGame", time);
    }

    public static void RemoveFromGame(GameObject victim)
    {
        GameBehaviour handler = victim.GetComponent<GameBehaviour>();
        if (handler)
            handler.RemoveFromGame();
        else
            RequestDefaultDestruction(victim);
    }

    protected virtual void RequestDestruction()
    {
        RequestDefaultDestruction(gameObject);
    }

    static void RequestDefaultDestruction(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    //public static void KillWithExplosion(GameObject victim)
    //public void KillWithExplosion(GameObject victim)
    //{
    //    //if (victim.tag == "Ship")
    //    //    Spawn.ShipExplosion(victim.transform.position);
    //    //else
    //    //    Spawn.AsteroidExplosion(victim.transform.position);

    //    //RemoveFromGame(victim);
    //    exploder.SpawnExplosion(victim.tag, victim.transform.position);
    //}

    protected void Score(int score)
    {
        global::Score.Earn(score);
    }

}
