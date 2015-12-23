using UnityEngine;
using System.Collections;

public class GameToken : GameBehaviour
{
    [SerializeField]
    [Range(0, 200)]
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
    public virtual void Spawn()
    {
        ApplySpawnVariance();
        transform.position = FindOpenPosition();
    }

    public virtual void SpawnAt(Vector3 position)
    {
        ApplySpawnVariance();
        transform.position = position;
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

    Vector3 FindOpenPosition(int layerMask = ~0)
    {
        float x = transform.localScale.x;
        float y = transform.localScale.y;
        float collisionSphereRadius = x > y ? x : y;
        //float collisionSphereRadius = GetComponent<Renderer>().bounds.size.magnitude / 4f;
        bool overlap = false;
        Vector3 openPosition;
        do
        {
            openPosition = Viewport.GetRandomWorldPositionXY();
            overlap = Physics.CheckSphere(openPosition, collisionSphereRadius, layerMask);
        } while (overlap);
        return openPosition;
    }
    #endregion
}
