using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    Poolable poolable;

    protected virtual void Awake()
    {
        poolable = GetComponent<Poolable>();
    }

    public void RemoveFromGame()
    {
        if (poolable)
            poolable.Recycle();
        else
            Destroy(gameObject);
    }

    public static void Kill(GameObject victim)
    {
        Spawn.Explosion(victim.transform.position);

        GameBehaviour victimBehaviour = victim.GetComponent<GameBehaviour>();
        if (victimBehaviour)
            victimBehaviour.RemoveFromGame();
        else
            victim.SetActive(false);
    }

    protected void Score(int score)
    {
        global::Score.Earn(score);
    }
}
