using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AsteroidBehaviour : GameBehaviour
{
    public Exploder exploder;

    [SerializeField]
    [Range(25, 500)]
    protected int destructionScore = 25;

    [SerializeField]
    [FormerlySerializedAs("m_Force")]
    float initialForce = 2000f;

    [SerializeField]
    [FormerlySerializedAs("m_Torque")]
    float initialTorque = 1000f;

#pragma warning disable 0649
    [SerializeField]
    UniformRandomVector3 uniformScale;
#pragma warning restore 0649

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

    #region Spawning
    public virtual void SpawnAt(Vector3 position)
    {
        transform.position = position;
        ApplySpawnVariance();
    }

    protected virtual void ApplySpawnVariance()
    {
        transform.localScale = uniformScale.Randomize();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody)
        {
            RigidbodyExt.SetRandomForce(rigidbody, initialForce);
            RigidbodyExt.SetRandomTorque(rigidbody, initialTorque);
        }
    }
    #endregion

    #region Hit by ship
    protected virtual void OnCollisionEnter(Collision otherCollision)
    {
        GameObject otherObject =  otherCollision.gameObject;
        if (otherObject.tag == "Ship")
            HitByShip(otherObject);
    }

    protected void HitByShip(GameObject ship)
    {
        KillWithExplosion(victim: ship);
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

    public static Vector3 FindSuitableSpawnLocation()
    {
        int mask = LayerMask.GetMask("ShipSpawnSphere");
        Vector3 spawnPosition;
        float collisionSphereRadius = 5f;
        bool hit = false;
        do
        {
            spawnPosition = Viewport.GetRandomWorldPositionXY();
            hit = Physics.CheckSphere(spawnPosition, collisionSphereRadius, mask);
        } while (hit);
        return spawnPosition;
    }
    #endregion
}