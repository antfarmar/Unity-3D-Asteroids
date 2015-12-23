using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Exploder", menuName = "Exploder")]
public class Exploder : ScriptableObject
{
    public GameObject m_BigExplosionPrefab;
    public GameObject m_SmallExplosionPrefab;
    public GameObject m_ShipExplosionPrefab;
    public GameObject m_UFOExplosionPrefab;

    // Scriptable Objects might not be a good use case with pools?!
    ObjectPool m_BigExplosionPool;
    ObjectPool m_SmallExplosionPool;
    GameObject shipExplosion;
    GameObject ufoExplosion;

    bool poolsBuilt;

    // Called in Editor too!
    void OnEnable()
    {
        //hideFlags = HideFlags.DontSave; // http://blogs.unity3d.com/2012/10/25/unity-serialization/   
    }

    // Called in Editor too!
    void OnDisable()
    {
        poolsBuilt = false;
        DestroyImmediate(m_BigExplosionPool);
        DestroyImmediate(m_SmallExplosionPool);
        shipExplosion = null;
        ufoExplosion = null;
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
            shipExplosion = Instantiate(m_ShipExplosionPrefab);
            ufoExplosion = Instantiate(m_UFOExplosionPrefab);
            shipExplosion.SetActive(false);
            ufoExplosion.SetActive(false);
            poolsBuilt = true;
        }
    }

    public void Explode(string tag, Vector3 position)
    {
        if (tag == "Ship")
            SpawnShipExplosion(position);
        else if (tag == "UFO")
            SpawnUFOExplosion(position);
        else
        {
            bool isAsteroidBig = (tag == "AsteroidBig");
            SpawnAsteroidExplosion(isAsteroidBig, position);
        }
    }

    void SpawnAsteroidExplosion(bool spawnBig, Vector3 position)
    {
        Poolable explosion = spawnBig ? m_BigExplosionPool.GetRecyclable() : m_SmallExplosionPool.GetRecyclable();
        SetPositionRotation(explosion.transform, position);
    }

    void SpawnShipExplosion(Vector3 position)
    {
        SetPositionRotation(shipExplosion.transform, position);
        shipExplosion.SetActive(true);
    }

    void SpawnUFOExplosion(Vector3 position)
    {
        SetPositionRotation(ufoExplosion.transform, position);
        ufoExplosion.SetActive(true);
    }

    void SetPositionRotation(Transform transform, Vector3 position)
    {
        transform.position = position;
        transform.Rotate(RandomEulerZ());
    }

    Vector3 RandomEulerZ() { return new Vector3(0f, 0f, UnityEngine.Random.Range(0, 360)); }
}
