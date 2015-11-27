using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    ParticleSystem m_explosionPS;
    AudioSource m_explosionAudio;
    Poolable m_Poolable;

    void Awake()
    {
        m_explosionPS = GetComponent<ParticleSystem>();
        m_explosionAudio = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        m_explosionPS.Play();
        m_explosionAudio.Play();

        m_Poolable = GetComponent<Poolable>();
        Invoke("PoolItem", m_explosionPS.startLifetime);
    }

    void PoolItem()
    {
        GameManager.instance.m_ExplosionPool.Push(m_Poolable);
    }
}
