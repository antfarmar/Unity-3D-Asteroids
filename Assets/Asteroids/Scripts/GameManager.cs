using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public PowerupManager powerupManager;

    public GameObject m_ShipPrefab;
    public GameObject m_UFOPrefab;
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
        bigAsteroidPool = ObjectPool.Build(m_AsteroidBigPrefab, 25, 50);
        smallAsteroidPool = ObjectPool.Build(m_AsteroidSmallPrefab, 25, 50);
        announce = GameAnnouncer.AnnounceTo(Announcer.TextComponent(m_UIText), Announcer.Log(this));
        wallpaper = AsteroidWallpaper.New(bigAsteroidPool, smallAsteroidPool);
    }

    void Start()
    {
        ship = Ship.Spawn(m_ShipPrefab);
        ship.RemoveFromGame();
        StartCoroutine(GameLoop());
        StartCoroutine(powerupManager.SpawnPowerupsFor(ship.gameObject));
        StartCoroutine(SpawnUFO());
    }

    void OnEnable() { instance = this; }

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
            GC.Collect();
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
        ship.Recover(); ship.EnableControls();
        announce.LevelStarts(level);
        yield return Pause.Long();
        SpawnAsteroids(numAsteroidsForLevel);
    }

    IEnumerator LevelPlay()
    {
        announce.LevelPlaying();
        while (ship.IsAlive && AsteroidBehaviour.Any) yield return null;
    }

    IEnumerator LevelEnd()
    {
        bool gameover = !ship.IsAlive;  //AsteroidBehaviour.Any;
        if (gameover)
        {
            announce.GameOver();
            yield return Pause.Brief(); Score.Tally();
            yield return Pause.Brief(); Score.Reset();
            RemoveRemainingGameTokens();
            powerupManager.DenyAllPower(); // ship should reset itself?
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
            asteroid.Spawn();
        }
    }

    public static void SpawnSmallAsteroid(Vector3 position)
    {
        AsteroidBehaviour asteroid = instance.smallAsteroidPool.GetRecyclable<AsteroidBehaviour>();
        asteroid.SpawnAt(position);
    }


    void RemoveRemainingGameTokens()
    {
        foreach (var a in FindObjectsOfType<GameToken>())
            a.RemoveFromGame();
    }

    #region UFO Testing
    IEnumerator SpawnUFO()
    {
        GameObject ufo = Instantiate(m_UFOPrefab);
        ufo.SetActive(false);
        ufo.GetComponent<UFO>().target = ship.transform;

        while (true)
        {
            var wait = UnityEngine.Random.Range(5f, 10f);
            yield return new WaitForSeconds(wait);
            if (ship.gameObject.activeSelf && !ufo.activeSelf)
            {
                ufo.SetActive(true);
            }
        }
    }
    #endregion
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