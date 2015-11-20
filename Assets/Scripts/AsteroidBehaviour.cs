using UnityEngine;


public class AsteroidBehaviour : MonoBehaviour
{

    //public GameObject m_AsteroidBigPrefab;
    //public GameObject m_AsteroidSmallPrefab;
    //public ObjectPooler m_AsteroidBigPool;
    //public ObjectPooler m_AsteroidSmallPool;

    public AudioClip m_ExplosionClip;


    public float m_Force = 2000f;
    public float m_Torque = 1000f;

    private Rigidbody m_Rigidbody;


    #region MonoBehaviours

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        SpawnRandomEdge();
    }


    void OnEnable()
    {
        SetRandomForces();
    }


    // Called when hit by a bullet trigger collider.
    void OnTriggerEnter(Collider other)
    {
        if(gameObject.CompareTag("AsteroidBig"))
        {
            // Split the big asteroid into 2 smaller ones.
            for(int i = 0; i < 2; i++)
            {
                //GameObject a1 = Instantiate(m_SmallAsteroidPrefab);
                Poolable small = GameManager.instance.m_AsteroidSmallPool.Pop();
                small.transform.position = gameObject.transform.position;
                small.gameObject.SetActive(true);
            }

            GameManager.instance.m_AsteroidBigPool.Push(gameObject.GetComponent("Poolable") as Poolable);
            SoundManager.instance.sfxSource.pitch = 1;
        }
        else // "AsteroidSmall"
        {
            GameManager.instance.m_AsteroidSmallPool.Push(gameObject.GetComponent("Poolable") as Poolable);
            SoundManager.instance.sfxSource.pitch = 2;
        }

        // Play the explosion sound clip via SM (because object will be destroyed).
        // Could also use  AudioSource.PlayClipAtPoint()
        SoundManager.instance.PlaySingle(m_ExplosionClip);

        // Activate an explosion.
        Poolable explosion = GameManager.instance.m_ExplosionPool.Pop();
        explosion.transform.position = transform.position;
        explosion.transform.Rotate(new Vector3(0f, 0f, 360f * Random.value));
        explosion.gameObject.SetActive(true);

        // Recycle & deactivate this asteroid.
        //GameManager.instance.m_AsteroidBigPool.Enqueue(gameObject.GetComponent("Poolable") as Poolable);
    }
    #endregion


    ///////////////////////// HELPER METHODS ////////////////////

    // Apply random forces.
    void SetRandomForces()
    {
        //Vector3 randomForce = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        //Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        //randomForce = m_Force * Vector3.Normalize(randomForce);
        //randomTorque = m_Torque * Vector3.Normalize(randomTorque);

        Vector3 randomTorque = m_Torque * Random.insideUnitSphere;
        Vector3 randomForce = m_Force * Random.insideUnitSphere;

        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.AddForce(randomForce);

        m_Rigidbody.angularVelocity = Vector3.zero;
        m_Rigidbody.AddTorque(randomTorque);
    }


    // Calculate a random spawn point along the screen edge.
    void SpawnRandomEdge()
    {
        Vector3 randomPosition = new Vector3(Random.value, Random.value, 0f);

        if(Random.value < 0.5f) // coin flip
            randomPosition.x = Random.value < 0.5f ? 0.1f : 0.9f;
        else
            randomPosition.y = Random.value < 0.5f ? 0.1f : 0.9f;

        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
        spawnPosition.z = 0f;

        transform.position = spawnPosition;

        // Add some small scale variation
        transform.localScale *= Random.Range(1f, 2f);
    }
}
