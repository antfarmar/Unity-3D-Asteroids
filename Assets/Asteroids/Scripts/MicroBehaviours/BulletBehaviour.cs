using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float m_BulletLife = 1f;
    ShipShooter m_ShipShooter;

    void Awake()
    {
        m_ShipShooter = FindObjectOfType<ShipShooter>();
    }

    void OnEnable()
    {
        Invoke("Repool", m_BulletLife);
    }

    void Repool()
    {
        m_ShipShooter.m_BulletPool.Recycle(GetComponent<Poolable>());
    }
}
