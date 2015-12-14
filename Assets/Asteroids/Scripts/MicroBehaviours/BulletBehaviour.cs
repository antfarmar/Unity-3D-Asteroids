using UnityEngine;

public class BulletBehaviour : GameBehaviour
{
    public float m_BulletLife = 1f;

    public virtual void Fire(Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        transform.position = position;
        transform.rotation = rotation;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = velocity;
    }

    void OnEnable()
    {
        InvokeRemoveFromGame(m_BulletLife);
    }
}
