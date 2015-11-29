using UnityEngine;


public class AsteroidBehaviour : MonoBehaviour
{

    //public GameObject m_AsteroidBigPrefab;
    //public GameObject m_AsteroidSmallPrefab;
    //public ObjectPooler m_AsteroidBigPool;
    //public ObjectPooler m_AsteroidSmallPool;

    public ParticleSystem m_ShipExplosionPS;
    public AudioClip m_ExplosionClip;


    public float m_Force = 2000f;
    public float m_Torque = 1000f;

    private Poolable m_Poolable;
    private Rigidbody m_Rigidbody;


    #region MonoBehaviours

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        // Add some small scale variation
        transform.localScale *= Random.Range(1f, 1.5f);
    }


    void OnEnable()
    {
        // Get Poolable component here. GObject won't have it on Awake! (change?)
        m_Poolable = GetComponent<Poolable>();
        //SetRandomForces();
    }


    // Called when hit by a bullet trigger collider.
    void OnTriggerEnter(Collider other)
    {
        // other = Bullet. Inactivate it until it repools itself.
        other.gameObject.SetActive(false);

        if(gameObject.CompareTag("AsteroidBig"))
        {
            // Split the big asteroid into 2 smaller ones.
            for(int i = 0; i < 2; i++)
            {
                //GameObject a1 = Instantiate(m_SmallAsteroidPrefab);
                Poolable small = GameManager.instance.m_AsteroidSmallPool.Pop();
                AsteroidBehaviour behaviour = small.GetComponent<AsteroidBehaviour>();
                small.transform.position = gameObject.transform.position;
                small.gameObject.SetActive(true);
                behaviour.SetRandomForces();

            }

            // Recycle this (big) asteroid.
            GameManager.instance.m_AsteroidBigPool.Push(m_Poolable);
        }
        else // "AsteroidSmall"
        {
            // Recycle this (small) asteroid.
            GameManager.instance.m_AsteroidSmallPool.Push(m_Poolable);
        }

        // Play the explosion sound clip via SM (because object will be destroyed).
        // Could also use  AudioSource.PlayClipAtPoint()
        //SoundManager.instance.PlaySingle(m_ExplosionClip);

        // Activate an explosion.
        SpawnExplosion();
        //Poolable explosion = GameManager.instance.m_ExplosionPool.Pop();
        //explosion.transform.position = transform.position;
        //explosion.transform.Rotate(new Vector3(0f, 0f, 360f * Random.value));
        //explosion.gameObject.SetActive(true);

    }
    #endregion


    // Ship is currently not a trigger.
    //void OnCollisionEnter(Collision collision)
    //{
    //    // Ship logically.
    //    collision.gameObject.SetActive(false);
    //    //SpawnExplosion(); // todo: make custom explosion for ship
    //    GameObject shipExplosion =
    //        Instantiate(m_ShipExplosionPS.gameObject, collision.transform.position, collision.transform.rotation) as GameObject;
    //    Destroy(shipExplosion, m_ShipExplosionPS.startLifetime);
    //}


    void SpawnExplosion()
    {
        Poolable explosion = GameManager.instance.m_AsteroidExplosionPool.Pop();
        explosion.transform.position = transform.position;
        explosion.transform.Rotate(new Vector3(0f, 0f, 360f * Random.value));
        explosion.gameObject.SetActive(true);
    }


    ///////////////////////// HELPER METHODS (make static?) ////////////////////


    // Apply random forces to the asteroid.
    public void SetRandomForces()
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


    // Spawn anywhere except on the ship's sphere collider.
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
        } while(hit);

        spawnPosition.z = 0f;
        transform.position = spawnPosition;
    }


    // Calculate a random spawn point along the screen edge.
    //public void SpawnRandomEdge()
    //{
    //    Vector3 randomPosition = new Vector3(Random.value, Random.value, 0f);

    //    // Restrict to an edge on x or y axis.
    //    if(Random.value < 0.5f) // coin flip
    //        randomPosition.x = Random.value < 0.5f ? 0.2f : 0.8f;
    //    else
    //        randomPosition.y = Random.value < 0.5f ? 0.2f : 0.8f;

    //    Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(randomPosition);
    //    spawnPosition.z = 0f;

    //    transform.position = spawnPosition;

    //}
}
