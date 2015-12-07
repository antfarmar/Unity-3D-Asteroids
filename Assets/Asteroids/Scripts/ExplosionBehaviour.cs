using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{

    private ParticleSystem m_ExplosionPS;
    private AudioSource m_ExplosionAudio;
    private Poolable m_Poolable;


    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        m_ExplosionPS = GetComponent<ParticleSystem>();
        m_ExplosionAudio = GetComponent<AudioSource>();
    }


    // Play the system, then recycle it when over.
    void OnEnable()
    {
        m_ExplosionPS.Play();
        m_ExplosionAudio.Play();

        // Get Poolable component here. GObject won't have it on Awake!
        m_Poolable = GetComponent<Poolable>();
        Invoke("PoolItem", m_ExplosionPS.startLifetime);
    }


    // Convenience method to be called by Invoke().
    void PoolItem()
    {
        GameManager.instance.m_AsteroidExplosionPool.Push(m_Poolable);
    }

}