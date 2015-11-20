using UnityEngine;
using System.Collections;

public class ExplosionBehaviour : MonoBehaviour
{

    private ParticleSystem m_explosionPS;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        m_explosionPS = GetComponent<ParticleSystem>();
    }


    // Play the system, then recycle it when over.
    void OnEnable()
    {
        m_explosionPS.Play();
        Invoke("PoolItem", m_explosionPS.startLifetime);
    }


    // Convenience method to be called by Invoke().
    void PoolItem()
    {
        ObjectPooler.Enqueue(GetComponent("Poolable") as Poolable);
    }

}