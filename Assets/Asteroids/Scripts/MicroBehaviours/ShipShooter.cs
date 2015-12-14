using UnityEngine;
using UnityEngine.Serialization;

public class ShipShooter : MonoBehaviour
{
    [FormerlySerializedAs("m_BulletPrefab")]
    public GameObject bulletPrefab;
    [FormerlySerializedAs("m_BulletPool")]
    public ObjectPool bulletPool;

    public enum Weapons { Default, Fast, Backwards, Spread, Count }
    public int currentWeapon = (int)Weapons.Default;

    AudioSource shootAudio;
    Transform nozzle;


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

    public void Shoot()
    {
        switch (currentWeapon)
        {
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

        shootAudio.Play();
    }

    void ShootDefault()
    {
        FireBullet(direction: nozzle.up, velocity: 25);
    }

    void ShootFast()
    {
        FireBullet(direction: nozzle.up, velocity: 50);
    }

    void ShootBackwards()
    {
        //Vector3 backwards = -nozzle.up;
        FireBullet(direction: nozzle.up, velocity: 25);
        FireBullet(direction: -nozzle.up, velocity: 25);
    }



    void ShootSpread()
    {
        float zDegrees = 15f;

        for (int i = -1; i <= 1; i++)
        {
            Vector3 direction = nozzle.up;
            direction = Quaternion.Euler(0, 0, zDegrees * i) * nozzle.up;
            FireBullet(direction: direction, velocity: 25);
        }

    }

    void FireBullet(Vector3 direction, float velocity)
    {
        // Note: This is a 2D game. "up" is treated as "forward" in 2D. 
        //Vector3 forward = nozzle.up;
        Bullet().Fire(nozzle.position, nozzle.rotation, direction * velocity);
    }

    BulletBehaviour Bullet()
    {
        return bulletPool.GetRecyclable<BulletBehaviour>();
    }
}
