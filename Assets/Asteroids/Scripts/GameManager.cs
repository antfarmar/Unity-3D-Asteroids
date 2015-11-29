using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject m_ShipPrefab;
    public GameObject m_BulletPrefab;
    public GameObject m_ExplosionPrefab;
    public GameObject m_AsteroidBigPrefab;
    public GameObject m_AsteroidSmallPrefab;
    public int m_AsteroidCount = 2;
    public Text m_UIText;
    static GameManager instance;

    ObjectPool bigAsteroidPool;
    ObjectPool smallAsteroidPool;
    ObjectPool explosionPool;

    GameObject m_Ship;
    int level = 1;
    bool m_AllAsteroidsShot = false;
    bool showTitleScreen = true;

    public static void SpawnSmallAsteroid(Vector3 position)
    {
        Poolable p = instance.smallAsteroidPool.GetRecyclable();
        AsteroidBehaviour asteroid = p.GetComponent<AsteroidBehaviour>();
        asteroid.SpawnAt(position);
    }

    public static void SpawnExplosion(Vector3 position)
    {
        Poolable explosion = instance.explosionPool.GetRecyclable();
        explosion.transform.position = position;
        explosion.transform.Rotate(new Vector3(0f, 0f, 360f * UnityEngine.Random.value));
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            AwakeContinued();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void AwakeContinued()
    {
        bigAsteroidPool = ObjectPool.Build(m_AsteroidBigPrefab, 10, 20);
        smallAsteroidPool = ObjectPool.Build(m_AsteroidSmallPrefab, 10, 30);
        explosionPool = ObjectPool.Build(m_ExplosionPrefab, 5, 5);
    }

    void OnEnable()
    {
        instance = this;
    }

    void Start()
    {
        // Spawn the ship & deactivate.
        m_Ship = Instantiate(m_ShipPrefab);
        m_Ship.transform.position = Vector3.zero;
        m_Ship.SetActive(false);
        level = 1;

        GC.Collect();
        StartCoroutine(GameLoop());
    }

    // The main game loop.
    IEnumerator GameLoop()
    {
        var delay = new WaitForSeconds(2f);
        while (true)
        {
            if (showTitleScreen)
                yield return StartCoroutine(ShowTitleScreen());
            yield return StartCoroutine(LevelStart());
            yield return StartCoroutine(LevelPlay());
            yield return StartCoroutine(LevelEnd());
            yield return delay;
        }
    }

    // Display a title screen with all asteroids active.
    // Wait for any key pressed to start the game.
    IEnumerator ShowTitleScreen()
    {
        showTitleScreen = false;
        m_UIText.text = "A S T E R O I D S";
        SpawnBackgroundAsteroids();
        while (!Input.anyKeyDown) yield return null;
        RemoveBackgroundAsteroids();
    }

    // Spawn asteroids for this level.
    IEnumerator LevelStart()
    {
        m_UIText.text = "Level " + level;
        m_Ship.SetActive(true);
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < m_AsteroidCount; i++)
        {
            var pool = m_AsteroidCount % 2 == 0 ? bigAsteroidPool : smallAsteroidPool;
            var asteroid = pool.GetRecyclable();
            AsteroidBehaviour behaviour = asteroid.GetComponent<AsteroidBehaviour>();
            behaviour.SpawnAt(FindAsteroidSpawnPoint());
        }
    }

    IEnumerator LevelPlay()
    {
        Debug.Log("LEVEL PLAYING");
        m_UIText.text = string.Empty;

        // Enable ship controls.
        m_Ship.SetActive(true);
        m_Ship.GetComponent<ShipMovement>().enabled = true;
        m_Ship.GetComponent<ShipShooter>().enabled = true;

        // No health system yet. Ship just deactivated on collision.
        m_AllAsteroidsShot = false;
        while (m_Ship.activeSelf && !m_AllAsteroidsShot)
        {
            m_AllAsteroidsShot = !AnyActiveAsteroid();
            yield return null;
        }
    }

    IEnumerator LevelEnd()
    {
        Debug.Log("LEVEL ENDING");

        if (m_AllAsteroidsShot)
        {
            m_UIText.text = "Level Cleared!";
            level++;
            m_AsteroidCount += level; // level progression
            yield return new WaitForSeconds(1f);
            Score.LevelCleared(level);
            yield return new WaitForSeconds(1f);
        }
        else // ship collided & deactivated
        {
            m_UIText.text = "GAME OVER";
            level = 1;
            m_AsteroidCount = 2;
            showTitleScreen = true;
            m_Ship.transform.position = Vector3.zero;
            m_Ship.GetComponent<Rigidbody>().velocity = Vector3.zero;
            yield return new WaitForSeconds(1f);
            Score.Tally();
            yield return new WaitForSeconds(1f);
            Score.Reset();
        }

    }

    void SpawnBackgroundAsteroids()
    {
        SpawnAllAsteroids(bigAsteroidPool);
        SpawnAllAsteroids(smallAsteroidPool);
    }

    static void SpawnAllAsteroids(ObjectPool pool)
    {
        while (!pool.IsEmpty)
        {
            var asteroid = pool.GetRecyclable();
            AsteroidBehaviour behaviour = asteroid.GetComponent<AsteroidBehaviour>();
            behaviour.SpawnAt(FindAsteroidSpawnPoint());
        }
    }

    static Vector3 FindAsteroidSpawnPoint()
    {
        int mask = LayerMask.GetMask("ShipSpawnSphere");
        Vector3 spawnPosition;
        bool hit = false;
        do
        {
            spawnPosition = Viewport.GetRandomWorldPositionXY();
            hit = Physics.CheckSphere(spawnPosition, 5f, mask);
        } while (hit);
        return spawnPosition;
    }

    static void RemoveBackgroundAsteroids()
    {
        foreach (var asteroid in GetAllAsteroids())
            asteroid.RemoveFromGame();
    }

    static AsteroidBehaviour[] GetAllAsteroids()
    {
        return FindObjectsOfType<AsteroidBehaviour>();
    }

    static bool AnyActiveAsteroid()
    {
        return GetAllAsteroids().Length > 0;
    }
}

