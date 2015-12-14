using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public PowerupManager powerupManager;

    public GameObject m_ShipPrefab;
    //public GameObject m_ExplosionPrefab;
    //public GameObject m_ShipExplosionPrefab;
    public GameObject m_AsteroidBigPrefab;
    public GameObject m_AsteroidSmallPrefab;
    public Text m_UIText;

    ObjectPool bigAsteroidPool;
    ObjectPool smallAsteroidPool;
    //ObjectPool explosionPool;

    AsteroidWallpaper wallpaper;
    GameAnnouncer announce;
    Ship ship;

    int level = 1;
    int numAsteroidsForLevel = 2;
    bool requestTitleScreen = true;


    void Awake()
    {
        SingletonInstanceGuard();
        bigAsteroidPool = ObjectPool.Build(m_AsteroidBigPrefab, 10, 20);
        smallAsteroidPool = ObjectPool.Build(m_AsteroidSmallPrefab, 10, 30);
        //explosionPool = ObjectPool.Build(m_ExplosionPrefab, 5, 5);
        announce = GameAnnouncer.AnnounceTo(Announcer.TextComponent(m_UIText), Announcer.Log(this));
        wallpaper = AsteroidWallpaper.New(bigAsteroidPool, smallAsteroidPool);
    }

    void Start()
    {
        ship = Ship.Spawn(m_ShipPrefab);
        ship.RemoveFromGame();
        GC.Collect();
        StartCoroutine(GameLoop());
    }

    void OnEnable()
    {
        instance = this;
    }

    void SingletonInstanceGuard()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            throw new SingletonException("Only one instance is allowed");
        }
    }

    IEnumerator GameLoop()
    {
        NewGame();
        while (true)
        {
            if (requestTitleScreen)
            {
                requestTitleScreen = false;
                yield return StartCoroutine(ShowTitleScreen());
            }
            yield return StartCoroutine(LevelStart());
            yield return StartCoroutine(LevelPlay());
            yield return StartCoroutine(LevelEnd());
        }
    }

    IEnumerator ShowTitleScreen()
    {
        announce.Title();
        wallpaper.ShowAsteroids();
        while (!Input.anyKeyDown) yield return null;
        wallpaper.HideAsteroids();
    }

    IEnumerator LevelStart()
    {
        ship.Recover();
        announce.LevelStarts(level);
        yield return Pause.Long();
        SpawnAsteroids(numAsteroidsForLevel);
    }

    IEnumerator LevelPlay()
    {
        ship.EnableControls();
        announce.LevelPlaying();
        Coroutine powerUpCoroutine = StartCoroutine(powerupManager.PowerupSpawner());
        while (ship.IsAlive && AsteroidBehaviour.Any) yield return null;
        StopCoroutine(powerUpCoroutine);
    }

    //IEnumerator TrySpawnPowerup()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(2f);
    //        powerupManager.ChanceSpawn();
    //    }

    //}

    IEnumerator LevelEnd()
    {
        bool gameover = AsteroidBehaviour.Any;
        if (gameover)
        {
            announce.GameOver();
            yield return Pause.Brief(); Score.Tally();
            yield return Pause.Brief(); Score.Reset();
            RemoveRemainingGameTokens();
            announce.ClearAnnouncements();
            NewGame();
        }
        else
        {
            announce.LevelCleared();
            yield return Pause.Brief(); Score.LevelCleared(level);
            yield return Pause.Brief();
            AdvanceLevel();
        }
        yield return Pause.Long();
    }

    void NewGame()
    {
        level = 1;
        numAsteroidsForLevel = 2;
        requestTitleScreen = true;
    }

    void AdvanceLevel()
    {
        level++;
        numAsteroidsForLevel += level; // Progression: 4, 7, 11, 16, (... +6, +7, +8, ...)
    }

    void SpawnAsteroids(int count)
    {
        for (int i = 0; i < count; i++)
        {
            ObjectPool bigOrSmall = i % 2 == 0 ? bigAsteroidPool : smallAsteroidPool;
            var asteroid = bigOrSmall.GetRecyclable<AsteroidBehaviour>();
            asteroid.SpawnAt(AsteroidBehaviour.FindAsteroidSpawnLocation());
        }
    }

    public static void SpawnSmallAsteroid(Vector3 position)
    {
        AsteroidBehaviour asteroid = instance.smallAsteroidPool.GetRecyclable<AsteroidBehaviour>();
        asteroid.SpawnAt(position);
    }

    //public static void SpawnAsteroidExplosion(Vector3 position)
    //{
    //    Poolable explosion = instance.explosionPool.GetRecyclable();
    //    explosion.transform.position = position;
    //    explosion.transform.Rotate(new Vector3(0f, 0f, UnityEngine.Random.Range(0, 360)));
    //}

    //public static void SpawnShipExplosion(Vector3 position)
    //{
    //    var rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360));
    //    Instantiate(instance.m_ShipExplosionPrefab, position, rotation);
    //}

    void RemoveRemainingGameTokens()
    {
        foreach (var a in FindObjectsOfType<GameToken>())
            a.RemoveFromGame();
    }
}

public static class Pause
{
    public static WaitForSeconds Long()
    {
        return new WaitForSeconds(2f);
    }

    public static WaitForSeconds Brief()
    {
        return new WaitForSeconds(1f);
    }
}