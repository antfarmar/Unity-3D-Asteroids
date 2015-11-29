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

    [SerializeField]
    UniformRandomVector3 uniformScale;

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
        Kill(ship);
    }
    #endregion

    #region Hit by Bullet
    protected virtual void OnTriggerEnter(Collider bulletCollider)
    {
        HitByBullet(bulletCollider.gameObject);
    }

    protected void HitByBullet(GameObject bullet)
    {
        Kill(bullet);
        Score(destructionScore);
        Shatter();
        RemoveFromGame();
    }

    protected virtual void Shatter()
    {
    }
    #endregion
}