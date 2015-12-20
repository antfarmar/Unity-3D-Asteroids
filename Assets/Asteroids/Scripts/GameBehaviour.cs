using UnityEngine;

public class GameBehaviour : MonoBehaviour, IPoolableAware, IRecyclable
{
    Poolable poolable;
    void IPoolableAware.PoolableAwoke(Poolable p) { poolable = p; }
    void IRecyclable.Recycle() { RemoveFromGame(); }

    protected void Score(int score) { global::Score.Earn(score); }

    protected virtual void OnDisable() { CancelInvokeRemoveFromGame(); }

    public void InvokeRemoveFromGame(float time) { Invoke("RemoveFromGame", time); }

    public void CancelInvokeRemoveFromGame() { CancelInvoke("RemoveFromGame"); }

    public void RemoveFromGame()
    {
        if (poolable)
            poolable.Recycle();
        else
            RequestDestruction();
    }

    public static void RemoveFromGame(GameObject victim)
    {
        GameBehaviour handler = victim.GetComponent<GameBehaviour>();
        if (handler)
            handler.RemoveFromGame();
        else
            RequestDefaultDestruction(victim);
    }

    protected virtual void RequestDestruction() { RequestDefaultDestruction(gameObject); }

    static void RequestDefaultDestruction(GameObject gameObject) { Destroy(gameObject); }
}
