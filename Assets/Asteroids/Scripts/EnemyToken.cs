using UnityEngine;
using System.Collections;

// This class is just collisions & explosions. Should be a Collision Manager?
public class EnemyToken : GameToken
{
    public static Exploder exploder;

    void Awake()
    {
        if (!exploder)
        {
            exploder = Resources.Load<Exploder>("Exploder");
            exploder.BuildPools();
        }
    }

    #region Hit by Ship/UFO
    protected virtual void OnCollisionEnter(Collision otherCollision)
    {
        GameObject otherObject = otherCollision.gameObject;
        if (otherObject.tag == "Ship")
            HitByShip(otherObject);
        else if (otherObject.tag == "UFO")
            HitByUFO(otherObject);
    }

    protected void HitByShip(GameObject ship)
    {
        if (!Ship.ActiveShield(ship)) KillWithExplosion(victim: ship);
    }

    protected void HitByUFO(GameObject ufo)
    {
        KillWithExplosion(victim: ufo);
    }


    #endregion

    #region Hit by Bullet
    protected virtual void OnTriggerEnter(Collider bulletCollider)
    {
        HitByBullet(bulletCollider.gameObject);
    }

    protected virtual void HitByBullet(GameObject bullet)
    {
        RemoveFromGame(bullet);
        KillWithExplosion(victim: this.gameObject);
        Score(destructionScore);
    }

    public void KillWithExplosion(GameObject victim)
    {
        exploder.Explode(victim.tag, victim.transform.position);
        RemoveFromGame(victim);
    }
    #endregion
}
