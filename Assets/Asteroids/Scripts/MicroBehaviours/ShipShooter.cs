using UnityEngine;
using UnityEngine.Serialization;

public class ShipShooter : MonoBehaviour
{
    [FormerlySerializedAs("m_BulletPrefab")]
    public GameObject bulletPrefab;
    [FormerlySerializedAs("m_BulletPool")]
    public ObjectPool bulletPool;

    AudioSource shootAudio;
    Transform nozzle;

    public void Shoot()
    {
        FireBullet(velocity: 25);
        shootAudio.Play();
    }

    void Awake()
    {
        shootAudio = GetComponent<AudioSource>();
        bulletPool = ObjectPool.Build(bulletPrefab, initialClones: 10, initialCapacity: 10);
        nozzle = transform.Find("BulletSpawnPoint");
    }

    void Update()
    {
        if (ShipInput.IsShooting())
            Shoot();
    }

    void FireBullet(float velocity)
    {
        // Note: This is a 2D game. "up" is treated as "forward" in 2D. 
        Vector3 forward = nozzle.up;
        Bullet().Fired(nozzle.position, nozzle.rotation, forward * velocity);
    }

    BulletBehaviour Bullet()
    {
        return bulletPool.GetRecyclable<BulletBehaviour>();
    }
}
