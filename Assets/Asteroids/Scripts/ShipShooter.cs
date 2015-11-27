using UnityEngine;

public class ShipShooter : MonoBehaviour
{
    public GameObject m_BulletPrefab;
    public ObjectPool m_BulletPool;
    public float m_BulletVelocity;

    AudioSource m_ShootingAudio;
    Transform m_BulletSpawnPoint;
    Poolable m_Bullet;


    void Awake()
    {
        m_ShootingAudio = GetComponent<AudioSource>();
        m_BulletPool = new ObjectPool(m_BulletPrefab, GameManager.instance.transform, 10, 10);
    }

    void Start()
    {
        m_BulletSpawnPoint = transform.Find("BulletSpawnPoint");
        m_BulletVelocity = 25f;
    }

    void Update()
    {
        if (ShipInput.IsShooting())
        {
            m_Bullet = m_BulletPool.Pop();
            Rigidbody rigidbody = m_Bullet.GetComponent<Rigidbody>();
            m_Bullet.transform.position = m_BulletSpawnPoint.position;
            m_Bullet.transform.rotation = m_BulletSpawnPoint.rotation;
            rigidbody.velocity = m_BulletVelocity * m_BulletSpawnPoint.up; //(up: y-axis)
            m_Bullet.gameObject.SetActive(true);
            m_ShootingAudio.Play();
        }
    }
}
