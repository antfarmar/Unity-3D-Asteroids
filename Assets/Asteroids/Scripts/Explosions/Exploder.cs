using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Exploder", menuName = "Exploder")]
public class Exploder : ScriptableObject
{
    public GameObject m_BigExplosionPrefab;
    public GameObject m_SmallExplosionPrefab;
    public GameObject m_ShipExplosionPrefab;

    // Scriptable Objects might not be a good use case with pools?!
    ObjectPool m_BigExplosionPool;
    ObjectPool m_SmallExplosionPool;

    bool poolsBuilt;

    // Called in Editor too!
    void OnEnable()
    {
        //Debug.Log("S.O. ENABLED");
        //hideFlags = HideFlags.DontSave; // http://blogs.unity3d.com/2012/10/25/unity-serialization/   
    }

    // Called in Editor too!
    void OnDisable()
    {
        //Debug.Log("S.O. DISABLED");
        poolsBuilt = false;
        DestroyImmediate(m_BigExplosionPool);
        DestroyImmediate(m_SmallExplosionPool);
        //Resources.UnloadUnusedAssets();
    }


    public void BuildPools()
    {
        if (!poolsBuilt)
        {
            m_BigExplosionPool = ObjectPool.Build(m_BigExplosionPrefab, 5, 5);
            m_SmallExplosionPool = ObjectPool.Build(m_SmallExplosionPrefab, 5, 5);
            m_BigExplosionPool.hideFlags = HideFlags.DontSave;
            m_SmallExplosionPool.hideFlags = HideFlags.DontSave;
            poolsBuilt = true;
            //Debug.Log("BUILT EXPLOSION POOLS");
        }
    }

    public void Explode(string tag, Vector3 position)
    {
        if (tag == "Ship")
        {
            SpawnShipExplosion(position);
        }
        else
        {
            bool isAsteroidBig = (tag == "AsteroidBig");
            SpawnAsteroidExplosion(isAsteroidBig, position);
        }
    }

    void SpawnAsteroidExplosion(bool spawnBig, Vector3 position)
    {
        Poolable explosion = spawnBig ? m_BigExplosionPool.GetRecyclable() : m_SmallExplosionPool.GetRecyclable();
        explosion.transform.position = position;
        explosion.transform.Rotate(new Vector3(0f, 0f, UnityEngine.Random.Range(0, 360)));
    }

    void SpawnShipExplosion(Vector3 position)
    {
        var rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360));
        Instantiate(m_ShipExplosionPrefab, position, rotation);
    }
}
