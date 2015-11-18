using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{

    public float m_BulletLife;


    // Recycle the bullet after each (re)activation when lifetime is over.
    void OnEnable()
    {
        Invoke("PoolItem", m_BulletLife);
    }


    // We hit something (asteroid). Recycle/deactivate the bullet.
    private void OnTriggerEnter(Collider other)
    {
        // Prevent recycling the bullet twice. (Pooler already checks for this)
        CancelInvoke("PoolItem");
        PoolItem();
    }


    // Convenience method to be called by Invoke().
    void PoolItem()
    {
        ObjectPooler.Enqueue(gameObject.GetComponent("Poolable") as Poolable);
    }

}