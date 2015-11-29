using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject m_ShipPrefab;
    public GameObject m_BulletPrefab;
    public GameObject m_AsteroidExplosionPrefab;
    public GameObject m_AsteroidBigPrefab;
    public GameObject m_AsteroidSmallPrefab;

    public int m_AsteroidCount = 2;
    //private bool paused;

    //public ObjectPool m_BulletPool;
    public ObjectPool m_AsteroidBigPool;
    public ObjectPool m_AsteroidSmallPool;
    public ObjectPool m_AsteroidExplosionPool;

    public Text m_UIText;

    public static GameManager instance;

    private Transform m_AsteroidParent;
    private GameObject m_Ship;
    private int m_Level = 1;
    private bool m_AllAsteroidsShot = false;
    private bool m_GameOver = true;


    void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        // Make/get references.
        m_AsteroidParent = new GameObject("Asteroids").transform;
        //m_BulletPool = new ObjectPool(m_BulletPrefab, gameObject.transform, 3, 5);
        m_AsteroidBigPool = new ObjectPool(m_AsteroidBigPrefab, m_AsteroidParent, 10, 20);
        m_AsteroidSmallPool = new ObjectPool(m_AsteroidSmallPrefab, m_AsteroidParent, 10, 30);
        m_AsteroidExplosionPool = new ObjectPool(m_AsteroidExplosionPrefab, gameObject.transform, 5, 5);
    }


    // Live-compilation calls onEnable when done.
    void OnEnable()
    {
        //Debug.Log("GM E1: " + instance);
        instance = this; // Reassign the lost static reference (static not serialized on live-recomp)
        //Debug.Log("GM E2:  " + instance);
    }

    // Live-compilation calls onDisable when starting.
    void OnDisable()
    {
        //Debug.Log("GM D: " + instance);
    }


    // Start is called before the first frame update only if the script instance is enabled.
    // Use this for initialization.
    void Start()
    {
        // Spawn the ship & deactivate.
        m_Ship = Instantiate(m_ShipPrefab);
        m_Ship.transform.position = Vector3.zero;
        m_Ship.SetActive(false);
        m_Level = 1;

        GC.Collect(); // clean garbage before starting game.
        StartCoroutine(GameLoop());
    }


    // The main game loop.
    private IEnumerator GameLoop()
    {
        if(m_GameOver) yield return StartCoroutine(ShowTitleScreen());
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

        while(!Input.anyKeyDown) yield return null;

        m_GameOver = false;

        PushAllAsteroids();

        GC.Collect(); // clean garbage before starting game.
        //yield return new WaitForSeconds(1f);
    }


    // Spawn asteroids for this level.
    IEnumerator LevelStart()
    {
        Debug.Log("LEVEL STARTING");
        m_UIText.text = "Level " + m_Level;

        // Disable ship controls (currently ship is inactive in Start, but may change).
        m_Ship.SetActive(true);
        //m_Ship.GetComponent<ShipMovement>().enabled = false;
        //m_Ship.GetComponent<ShipShooter>().enabled = false;

        // Reset ship position/velocity.
        //m_Ship.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //m_Ship.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //m_Ship.transform.position = Vector3.zero;


        yield return new WaitForSeconds(2f);


        // Spawn some asteroids.
        Poolable asteroid;
        for(int i = 0; i < m_AsteroidCount; i++)
        {
            if(i < m_AsteroidCount / 2)
                asteroid = m_AsteroidBigPool.Pop();
            else
                asteroid = m_AsteroidSmallPool.Pop();

            //AsteroidBehaviour behaviour = asteroid.GetComponent<AsteroidBehaviour>();
            //asteroid.gameObject.SetActive(true);
            //behaviour.SpawnRandomPosition();
            //behaviour.SetRandomForces();

            asteroid.gameObject.SetActive(true);
            AsteroidBaseBehaviour.SpawnRandomPosition(asteroid.gameObject);
            AsteroidBaseBehaviour.SetRandomForces(asteroid.gameObject);
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
        while(m_Ship.activeSelf && !m_AllAsteroidsShot)
        {
            m_AllAsteroidsShot = !AnyActiveAsteroid();
            yield return null;
        }
    }


    IEnumerator LevelEnd()
    {
        Debug.Log("LEVEL ENDING");

        if(m_AllAsteroidsShot)
        {
            m_UIText.text = "Level Cleared!";
            m_Level++;
            m_AsteroidCount += m_Level; // level progression
        }
        else // ship collided & deactivated
        {
            m_UIText.text = "GAME OVER";
            m_Level = 1;
            m_AsteroidCount = 2;
            m_GameOver = true;
            m_Ship.transform.position = Vector3.zero;
            m_Ship.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        yield return new WaitForSeconds(1f);
    }


    // Pop all asteroids out of their pools. (also sets position/forces)
    void PopAllAsteroids()
    {
        int count = m_AsteroidParent.childCount;
        for(int i = 0; i < count; i++)
        {
            GameObject asteroid = m_AsteroidParent.GetChild(i).gameObject;

            if(asteroid.CompareTag("AsteroidBig"))
                m_AsteroidBigPool.Pop();
            else
                m_AsteroidSmallPool.Pop();

            //AsteroidBehaviour behaviour = asteroid.GetComponent<AsteroidBehaviour>();
            asteroid.SetActive(true);
            AsteroidBaseBehaviour.SpawnRandomPosition(asteroid);
            AsteroidBaseBehaviour.SetRandomForces(asteroid);
        }
    }


    // Push all asteroids back into their pools. (Deactivates)
    void PushAllAsteroids()
    {
        int count = m_AsteroidParent.childCount;
        for(int i = 0; i < count; i++)
        {
            GameObject asteroid = m_AsteroidParent.GetChild(i).gameObject;
            Poolable poolable = asteroid.GetComponent<Poolable>();

            if(asteroid.CompareTag("AsteroidBig"))
                m_AsteroidBigPool.Push(poolable);
            else
                m_AsteroidSmallPool.Push(poolable);
        }
    }


    // Check if any asteroids are still active in the scene.
    bool AnyActiveAsteroid()
    {
        int count = m_AsteroidParent.childCount;
        for(int i = 0; i < count; i++)
        {
            GameObject asteroid = m_AsteroidParent.GetChild(i).gameObject;
            if(asteroid.activeSelf)
            {
                return true;
            }
        }
        return false;
    }


} // end class
