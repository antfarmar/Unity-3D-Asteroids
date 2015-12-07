using UnityEngine;


public class AsteroidBigBehaviour : AsteroidBaseBehaviour
{

    // Called when hit by a bullet trigger collider.
    public override void HandleTriggerEnter(Collider other)
    {
        // other == Bullet. Deactivate it until it repools itself.
        other.gameObject.SetActive(false);

        // Split the big asteroid into 2 smaller ones.
        for(int i = 0; i < 2; i++)
        {
            //GameObject a1 = Instantiate(m_SmallAsteroidPrefab);
            Poolable small = GameManager.instance.m_AsteroidSmallPool.Pop();
            //AsteroidBaseBehaviour behaviour = small.GetComponent<AsteroidBaseBehaviour>();
            small.transform.position = gameObject.transform.position;
            small.gameObject.SetActive(true);
            SetRandomForces(small.gameObject);

        }

        // Recycle this big asteroid.
        GameManager.instance.m_AsteroidBigPool.Push(m_Poolable);

        // Activate an explosion.
        SpawnExplosion();
    }
}