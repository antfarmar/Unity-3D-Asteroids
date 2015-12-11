using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public GameObject m_ShipPrefab;
    public GameObject m_AsteroidBigPrefab;
    public GameObject m_AsteroidSmallPrefab;
    public Text m_UIText;

    ObjectPool bigAsteroidPool;
    ObjectPool smallAsteroidPool;

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
        while (ship.IsAlive && AsteroidBehaviour.Any) yield return null;
    }

    IEnumerator LevelEnd()
    {
        bool gameover = AsteroidBehaviour.Any;
        if (gameover)
        {
            announce.GameOver();
            yield return Pause.Brief(); Score.Tally();
            yield return Pause.Brief(); Score.Reset();
            RemoveRemainingAsteroids();
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
            //bool decision = UnityEngine.Random.value > 0.5f;
            bool decision = i % 2 == 0;
            ObjectPool bigOrSmall = decision ? bigAsteroidPool : smallAsteroidPool;
            var asteroid = bigOrSmall.GetRecyclable<AsteroidBehaviour>();
            asteroid.SpawnAt(AsteroidBehaviour.FindSuitableSpawnLocation());
        }
    }

    public static void SpawnSmallAsteroid(Vector3 position)
    {
        AsteroidBehaviour asteroid = instance.smallAsteroidPool.GetRecyclable<AsteroidBehaviour>();
        asteroid.SpawnAt(position);
    }

    void RemoveRemainingAsteroids()
    {
        foreach (var a in FindObjectsOfType<AsteroidBehaviour>())
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