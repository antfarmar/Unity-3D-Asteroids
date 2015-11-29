using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    ParticleSystem m_explosionPS;
    AudioSource m_explosionAudio;

    void Awake()
    {
        m_explosionPS = GetComponent<ParticleSystem>();
        m_explosionAudio = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        m_explosionPS.Play();
        m_explosionAudio.Play();

        Invoke("PoolItem", m_explosionPS.startLifetime);
    }

    void PoolItem()
    {
        var poolable = GetComponent<Poolable>();
        if (poolable)
            poolable.Recycle();        
    }
}
