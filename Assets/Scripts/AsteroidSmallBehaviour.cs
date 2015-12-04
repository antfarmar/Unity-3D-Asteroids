using UnityEngine;


public class AsteroidSmallBehaviour : AsteroidBaseBehaviour
{

    // Called when hit by a bullet trigger collider.
    public override void HandleTriggerEnter(Collider other)
    {
        // other == Bullet. Deactivate it until it repools itself.
        other.gameObject.SetActive(false);

        // Recycle this (big) asteroid.
        GameManager.instance.m_AsteroidSmallPool.Push(m_Poolable);

        // Activate an explosion.
        SpawnExplosion();
    }
}