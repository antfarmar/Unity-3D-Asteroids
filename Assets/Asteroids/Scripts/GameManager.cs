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
    public ObjectPool m_AsteroidBigPool;
    public ObjectPool m_AsteroidSmallPool;
    public ObjectPool m_ExplosionPool;
    public Text m_UIText;
    public static GameManager instance;

    Transform m_AsteroidParent;
    GameObject m_Ship;
    int m_Level = 1;
    bool m_AllAsteroidsShot = false;
    bool m_GameOver = true;

    const int levelClearedBonusScore = 100;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        m_AsteroidParent = new GameObject("Asteroids").transform;
        m_AsteroidBigPool = new ObjectPool(m_AsteroidBigPrefab, m_AsteroidParent, 10, 20);
        m_AsteroidSmallPool = new ObjectPool(m_AsteroidSmallPrefab, m_AsteroidParent, 10, 30);
        m_ExplosionPool = new ObjectPool(m_ExplosionPrefab, gameObject.transform, 5, 5);
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
        m_Level = 1;

        GC.Collect();
        StartCoroutine(GameLoop());
    }

    // The main game loop.
    IEnumerator GameLoop()
    {
        if (m_GameOver) yield return StartCoroutine(ShowTitleScreen());
        yield return StartCoroutine(LevelStart());
        yield return StartCoroutine(LevelPlay());
        yield return StartCoroutine(LevelEnd());
        yield return new WaitForSeconds(2f);
        StartCoroutine(GameLoop());
    }

    // Display a title screen with all asteroids active.
    // Wait for any key pressed to start the game.
    IEnumerator ShowTitleScreen()
    {
        PopAllAsteroids();
        m_UIText.text = "A S T E R O I D S";
        while (!Input.anyKeyDown) yield return null;
        m_GameOver = false;
        PushAllAsteroids();
    }


    // Spawn asteroids for this level.
    IEnumerator LevelStart()
    {
        Debug.Log("LEVEL STARTING");
        m_UIText.text = "Level " + m_Level;
        m_Ship.SetActive(true);
        yield return new WaitForSeconds(2f);

        Poolable asteroid;
        for (int i = 0; i < m_AsteroidCount; i++)
        {
            if (i < m_AsteroidCount / 2)
                asteroid = m_AsteroidBigPool.Pop();
            else
                asteroid = m_AsteroidSmallPool.Pop();

            AsteroidBehaviour behaviour = asteroid.GetComponent<AsteroidBehaviour>();
            asteroid.gameObject.SetActive(true);
            behaviour.SpawnRandomPosition();
            behaviour.SetRandomForces();
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
            m_Level++;
            m_AsteroidCount += m_Level; // level progression
            int endOfLevelBonus = m_Level * levelClearedBonusScore;
            yield return new WaitForSeconds(1f);
            Score.Earn(endOfLevelBonus);
            yield return new WaitForSeconds(1f);
        }
        else // ship collided & deactivated
        {
            m_UIText.text = "GAME OVER";
            m_Level = 1;
            m_AsteroidCount = 2;
            m_GameOver = true;
            m_Ship.transform.position = Vector3.zero;
            m_Ship.GetComponent<Rigidbody>().velocity = Vector3.zero;
            yield return new WaitForSeconds(1f);
            Score.Tally();
            yield return new WaitForSeconds(1f);
            Score.Reset();
        }

    }

    void PopAllAsteroids()
    {
        int count = m_AsteroidParent.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject asteroid = m_AsteroidParent.GetChild(i).gameObject;

            if (asteroid.CompareTag("AsteroidBig"))
                m_AsteroidBigPool.Pop();
            else
                m_AsteroidSmallPool.Pop();

            AsteroidBehaviour behaviour = asteroid.GetComponent<AsteroidBehaviour>();
            asteroid.SetActive(true);
            behaviour.SpawnRandomPosition();
            behaviour.SetRandomForces();
        }
    }

    void PushAllAsteroids()
    {
        int count = m_AsteroidParent.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject asteroid = m_AsteroidParent.GetChild(i).gameObject;
            Poolable poolable = asteroid.GetComponent<Poolable>();

            if (asteroid.CompareTag("AsteroidBig"))
                m_AsteroidBigPool.Push(poolable);
            else
                m_AsteroidSmallPool.Push(poolable);
        }
    }

    bool AnyActiveAsteroid()
    {
        int count = m_AsteroidParent.childCount;
        for (int i = 0; i < count; i++)
        {
            GameObject asteroid = m_AsteroidParent.GetChild(i).gameObject;
            if (asteroid.activeSelf)
                return true;
        }
        return false;
    }
}

