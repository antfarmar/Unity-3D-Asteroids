using UnityEngine;
using System.Collections;

public class GameToken : GameBehaviour
{
    [SerializeField]
    [Range(25, 500)]
    protected int destructionScore = 100;

    [SerializeField]
    float initialForce = 100f;

    [SerializeField]
    float initialTorque = 100f;

#pragma warning disable 0649
    [SerializeField]
    UniformRandomVector3 uniformScale;
#pragma warning restore 0649


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
}
