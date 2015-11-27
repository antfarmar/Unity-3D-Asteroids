using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    public AudioClip m_ExplosionClip;
    public float m_Force = 2000f;
    public float m_Torque = 1000f;

    Poolable m_Poolable;
    Rigidbody m_Rigidbody;

    const int smallAsteroidScore = 25;
    const int largeAsteroidScore = 100;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        transform.localScale *= Random.Range(1f, 1.5f);
    }

    void OnEnable()
    {
        m_Poolable = GetComponent<Poolable>();
    }

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);

        if (gameObject.CompareTag("AsteroidBig"))
        {
            Score.Earn(largeAsteroidScore);
            // Split the big asteroid into 2 smaller ones.
            for (int i = 0; i < 2; i++)
            {
                Poolable small = GameManager.instance.m_AsteroidSmallPool.Pop();
                AsteroidBehaviour behaviour = small.GetComponent<AsteroidBehaviour>();
                small.transform.position = gameObject.transform.position;
                small.gameObject.SetActive(true);
                behaviour.SetRandomForces();

            }
            GameManager.instance.m_AsteroidBigPool.Push(m_Poolable);
        }
        else // "AsteroidSmall"
        {
            Score.Earn(smallAsteroidScore);
            GameManager.instance.m_AsteroidSmallPool.Push(m_Poolable);
        }

        SpawnExplosion();
    }

    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.SetActive(false);
        SpawnExplosion();
    }


    void SpawnExplosion()
    {
        Poolable explosion = GameManager.instance.m_ExplosionPool.Pop();
        explosion.transform.position = transform.position;
        explosion.transform.Rotate(new Vector3(0f, 0f, 360f * Random.value));
        explosion.gameObject.SetActive(true);
    }

    public void SetRandomForces()
    {
        Vector3 randomTorque = m_Torque * Random.insideUnitSphere;
        Vector3 randomForce = m_Force * Random.insideUnitSphere;

        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.AddForce(randomForce);

        m_Rigidbody.angularVelocity = Vector3.zero;
        m_Rigidbody.AddTorque(randomTorque);
    }

    public void SpawnRandomPosition()
    {
        int mask = LayerMask.GetMask("ShipSpawnSphere");
        Vector3 spawnPosition;
        bool hit = false;

        do
        {
            Vector3 randomPosition = new Vector3(Random.value, Random.value, 0f);
            spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
            hit = Physics.CheckSphere(spawnPosition, 5f, mask);
        } while (hit);

        spawnPosition.z = 0f;
        transform.position = spawnPosition;
    }
}
