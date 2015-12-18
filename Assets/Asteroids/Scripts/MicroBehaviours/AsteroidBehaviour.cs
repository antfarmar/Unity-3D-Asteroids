using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AsteroidBehaviour : GameToken
{
    public Exploder exploder;

    static int activeCount;
    public static bool Any { get { return activeCount > 0; } }

    void Awake()
    {
        exploder.BuildPools();
    }

    protected virtual void OnEnable()
    {
        ++activeCount;
    }

    protected override void OnDisable()
    {
        --activeCount;
        base.OnDisable();
    }

    #region Hit by ship
    protected virtual void OnCollisionEnter(Collision otherCollision)
    {
        GameObject otherObject = otherCollision.gameObject;
        if (otherObject.tag == "Ship")
            HitByShip(otherObject);
    }

    protected void HitByShip(GameObject ship)
    {
        if (!ActiveShield(ship)) KillWithExplosion(victim: ship);
    }

    protected bool ActiveShield(GameObject ship)
    {
        return ship.transform.FindChild("Shield").gameObject.activeSelf;
    }
    #endregion

    #region Hit by Bullet
    protected virtual void OnTriggerEnter(Collider bulletCollider)
    {
        HitByBullet(bulletCollider.gameObject);
    }

    protected void HitByBullet(GameObject bullet)
    {
        RemoveFromGame(bullet);
        KillWithExplosion(victim: this.gameObject);
        Score(destructionScore);
        Shatter();
    }

    public void KillWithExplosion(GameObject victim)
    {
        exploder.Explode(victim.tag, victim.transform.position);
        RemoveFromGame(victim);
    }

    protected virtual void Shatter()
    {
    }

    public static Vector3 FindAsteroidSpawnLocation()
    {
        int mask = LayerMask.GetMask("ShipSpawnSphere");
        float collisionSphereRadius = 5f;
        return Spawn.FindSuitableSpawnLocation(mask, collisionSphereRadius);
    }
    #endregion
}