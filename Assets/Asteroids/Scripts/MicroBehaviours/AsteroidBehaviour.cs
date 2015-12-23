using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AsteroidBehaviour : EnemyToken
{
    static int activeCount;
    public static bool Any { get { return activeCount > 0; } }


    protected virtual void OnEnable()
    {
        ++activeCount;
    }

    protected override void OnDisable()
    {
        --activeCount;
        base.OnDisable();
    }

    protected override void HitByBullet(GameObject bullet)
    {
        Shatter();
        base.HitByBullet(bullet);
    }

    protected virtual void Shatter()
    {
    }
}