using UnityEngine;


public abstract class AsteroidBaseBehaviour : MonoBehaviour
{

    public float m_Force;   // = 2000f;
    public float m_Torque;  // = 1000f;

    protected Poolable m_Poolable;


    #region MonoBehaviours

    void Awake()
    {
        //m_Rigidbody = GetComponent<Rigidbody>();
        transform.localScale *= Random.Range(1f, 1.5f); // Add some small scale variation
    }


    void OnEnable()
    {
        // Get Poolable component here. GObject won't have it on Awake! (change?)
        m_Poolable = GetComponent<Poolable>();
    }


    // Called when hit by a bullet trigger collider.
    void OnTriggerEnter(Collider otherCollider)
    {
        HandleTriggerEnter(otherCollider);
    }

    #endregion


    public abstract void HandleTriggerEnter(Collider collider);



    ///////////////////////// HELPER METHODS (make static?) ////////////////////


    protected void SpawnExplosion()
    {
        Poolable explosion = GameManager.instance.m_AsteroidExplosionPool.Pop();
        explosion.transform.position = transform.position;
        explosion.transform.Rotate(new Vector3(0f, 0f, 360f * Random.value));
        explosion.gameObject.SetActive(true);
    }


    // Apply random forces to the asteroid.
    public static void SetRandomForces(GameObject asteroid)
    {
        //Vector3 randomForce = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        //Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        //randomForce = m_Force * Vector3.Normalize(randomForce);
        //randomTorque = m_Torque * Vector3.Normalize(randomTorque);

        AsteroidBaseBehaviour behaviour = asteroid.GetComponent<AsteroidBaseBehaviour>();
        Vector3 randomTorque = behaviour.m_Torque * Random.insideUnitSphere;
        Vector3 randomForce = behaviour.m_Force * Random.insideUnitSphere;

        Rigidbody rb = asteroid.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddForce(randomForce);

        rb.angularVelocity = Vector3.zero;
        rb.AddTorque(randomTorque);
    }


    // Spawn anywhere except on the ship's sphere collider.
    public static void SpawnRandomPosition(GameObject asteroid)
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
        asteroid.transform.position = spawnPosition;
    }

}
