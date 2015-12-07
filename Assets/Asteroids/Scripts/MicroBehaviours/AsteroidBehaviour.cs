using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AsteroidBehaviour : GameBehaviour
{
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

    protected virtual void OnEnable()
    {
        ++activeCount;
    }

    protected virtual void OnDisable()
    {
        --activeCount;
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
    protected virtual void OnCollisionEnter(Collision shipCollision)
    {
        HitByShip(shipCollision.gameObject);
    }

    protected void HitByShip(GameObject ship)
    {
        //KillWithExplosion(victim: ship);
        GameManager.SpawnShipExplosion(ship);
        RemoveFromGame(ship);
    }
    #endregion

    #region Hit by Bullet
    protected virtual void OnTriggerEnter(Collider bulletCollider)
    {
        HitByBullet(bulletCollider.gameObject);
    }

    protected void HitByBullet(GameObject bullet)
    {
        KillWithExplosion(victim: bullet);
        Score(destructionScore);
        Shatter();
        RemoveFromGame();
    }

    protected virtual void Shatter()
    {
    }

    public static Vector3 FindSuitableSpawnLocation()
    {
        int mask = LayerMask.GetMask("ShipSpawnSphere");
        Vector3 spawnPosition;
        bool hit = false;
        do
        {
            spawnPosition = Viewport.GetRandomWorldPositionXY();
            hit = Physics.CheckSphere(spawnPosition, 5f, mask);
        } while (hit);
        return spawnPosition;
    }
    #endregion
}