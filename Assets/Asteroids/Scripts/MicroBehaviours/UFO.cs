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
        Spawn();
        UFOAudio.Play();
        InvokeRepeating("FireRandomDirection", fireRate, fireRate);
    }

    protected override void OnDisable()
    {
        UFOAudio.Stop();
        CancelInvoke("FireRandomDirection");
        base.OnDisable();
    }

    protected override void RequestDestruction()
    {
        gameObject.SetActive(false);
    }

    void Update() { RotateOnYAxis(); }
    void RotateOnYAxis() { transform.Rotate(Vector3.up * 180f * Time.deltaTime); }

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

    void FireRandomDirection()
    {
        shootAudio.Play();
        FireBullet(Vector3.Normalize(Random.insideUnitCircle));
    }

    // Duplicated code from ShipShooter.
    void FireBullet(Vector3 direction, float speedScalar = bulletSpeed)
    {
        direction = (direction * speedScalar);
        Bullet().Fire(transform.position, transform.rotation, direction);
    }

    BulletBehaviour Bullet() { return bulletPool.GetRecyclable<BulletBehaviour>(); }
}
