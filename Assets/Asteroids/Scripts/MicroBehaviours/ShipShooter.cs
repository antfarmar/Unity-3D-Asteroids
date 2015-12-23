using UnityEngine;
using UnityEngine.Serialization;

public class ShipShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public AudioSource shootAudio;

    public enum Weapons { Default, Fast, Backwards, Spread, Count }
    public int activeWeapon = (int)Weapons.Default;
    const int bulletSpeed = 25;

    ObjectPool bulletPool;
    Rigidbody rb;
    Transform nozzle;
    Vector3 forward() { return nozzle.up; } // Note: This is a 2D game. "up" is treated as "forward" in 2D. 

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bulletPool = ObjectPool.Build(bulletPrefab, initialClones: 10, initialCapacity: 10);
        nozzle = transform.Find("BulletSpawnPoint");
    }

    public void OnEnable() { activeWeapon = (int)Weapons.Default; }

    void Update() { if (ShipInput.IsShooting()) Shoot(); }

    public void Shoot()
    {
        shootAudio.Play();
        switch (activeWeapon)
        {
            case (int)Weapons.Default:
                ShootDefault();
                break;
            case (int)Weapons.Fast:
                ShootFast();
                break;
            case (int)Weapons.Backwards:
                ShootBackwards();
                break;
            case (int)Weapons.Spread:
                ShootSpread();
                break;
            default:
                ShootDefault();
                break;
        }
    }

    void ShootDefault() { FireBullet(forward()); }

    void ShootFast() { FireBullet(forward(), bulletSpeed * 2); }

    void ShootBackwards() { ShootDefault(); FireBullet(-forward()); }

    void ShootSpread()
    {
        for (int i = -1; i <= 1; i++)
        {
            float zDegrees = 15f;
            Vector3 direction = Quaternion.Euler(0, 0, i * zDegrees) * forward();
            FireBullet(direction);
        }

    }

    void FireBullet(Vector3 direction, float speedScalar = bulletSpeed)
    {
        direction = (direction * speedScalar) + rb.velocity;
        Bullet().Fire(nozzle.position, nozzle.rotation, direction);
    }

    BulletBehaviour Bullet() { return bulletPool.GetRecyclable<BulletBehaviour>(); }
}
