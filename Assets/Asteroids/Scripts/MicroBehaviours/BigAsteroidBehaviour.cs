using UnityEngine;

public class BigAsteroidBehaviour : AsteroidBehaviour
{
    [Range(2, 5)]
    [SerializeField]
    protected int fragments = 2;

    protected virtual void Reset()
    {
        destructionScore = 100;
    }

    protected override void Shatter()
    {
        for (int i = 0; i < fragments; ++i)
            GameManager.SpawnSmallAsteroid(transform.position);
    }
}
