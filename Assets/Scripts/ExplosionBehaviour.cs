using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{

    private ParticleSystem m_explosionPS;
    private AudioSource m_explosionAudio;
    private Poolable m_Poolable;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        m_explosionPS = GetComponent<ParticleSystem>();
        m_explosionAudio = GetComponent<AudioSource>();
    }


    // Play the system, then recycle it when over.
    void OnEnable()
    {
        m_explosionPS.Play();
        m_explosionAudio.Play();

        // Get Poolable component here. GObject won't have it on Awake!
        m_Poolable = GetComponent<Poolable>();
        Invoke("PoolItem", m_explosionPS.startLifetime);
    }


    // Convenience method to be called by Invoke().
    void PoolItem()
    {
        GameManager.instance.m_ExplosionPool.Push(m_Poolable);
    }

}