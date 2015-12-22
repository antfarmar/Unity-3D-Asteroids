using UnityEngine;
using System.Collections;

public class UFO : EnemyToken
{
    public GameObject bulletPrefab;
    public AudioSource shootAudio;
    public AudioSource UFOAudio;
    public Transform target;
    public float maxSpeed;
    public float trackingSpeed;

    float fireRate = 1f; // 1/level?
    const int bulletSpeed = 25;

    ObjectPool bulletPool;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bulletPool = ObjectPool.Build(bulletPrefab, initialClones: 10, initialCapacity: 10);
    }

    void OnEnable()
    {
        Respawn();
        InvokeRepeating("FireRandomDirection", fireRate, fireRate);
        UFOAudio.Play();
    }

    protected override void OnDisable()
    {
        CancelInvoke("FireRandomDirection");
        UFOAudio.Stop();
        base.OnDisable();
    }

    protected override void RequestDestruction()
    {
        gameObject.SetActive(false);
    }

    void Update() { RotateOnYAxis(); }
    void RotateOnYAxis() { transform.Rotate(2f * Vector3.up); }

    void FixedUpdate()
    {
        if (target)
        {
            float step = trackingSpeed * Time.fixedDeltaTime;
            Vector3 toward = Vector3.MoveTowards(rb.position, target.position, step);
            rb.AddForce(target.position - rb.position);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            Debug.DrawLine(rb.position, toward);
        }
    }

    // Duplicate code copied from Powerup.
    void Respawn()
    {
        int mask = LayerMask.GetMask("Asteroid");
        float collisionSphereRadius = transform.localScale.x;
        Vector3 position = Spawn.FindSuitableSpawnLocation(mask, collisionSphereRadius);
        SpawnAt(position);
    }


    void FireRandomDirection()
    {
        FireBullet(Vector3.Normalize(Random.insideUnitCircle));
        shootAudio.Play();
    }

    // Duplicated code from ShipShooter.
    void FireBullet(Vector3 direction, float speedScalar = bulletSpeed)
    {
        direction = (direction * speedScalar);
        Bullet().Fire(transform.position, transform.rotation, direction);
    }

    BulletBehaviour Bullet() { return bulletPool.GetRecyclable<BulletBehaviour>(); }
}
