using System.Collections;
using UnityEngine;

public class ShipShooter : MonoBehaviour
{

    public GameObject m_BulletPrefab;
    public ObjectPool m_BulletPool;
    public float m_BulletVelocity;
    //public float m_BulletLife;


    //public AudioClip m_ShootClip;       

    private AudioSource m_ShootingAudio;
    private Transform m_BulletSpawnPoint;
    private Poolable m_Bullet;


    // Get references.
    void Awake()
    {
        m_ShootingAudio = GetComponent<AudioSource>();
        m_BulletPool = new ObjectPool(m_BulletPrefab, GameManager.instance.transform, 10, 10);
    }

    // Only called if the Object is active. This function is called just after the object is enabled.
    // This happens when a MonoBehaviour instance is created, such as when a level is loaded or a GameObject with the script component is instantiated.
    void OnEnable()
    {

    }


    // Start is called before the first frame update only if the script instance is enabled.
    // Use this for initialization.
    void Start()
    {
        // Create a reserve pool of bullets for use.
        //ObjectPooler.CreatePool("BulletPool", m_BulletPrefab, 5, 10);

        // This is probably more robust than below (child could change index).
        //m_BulletSpawnPoint = GameObject.FindGameObjectWithTag("BulletSpawnPoint").GetComponent<Transform>();
        m_BulletSpawnPoint = transform.Find("BulletSpawnPoint");
        m_BulletVelocity = 25f;
        //m_BulletLife = 1f;

        //Assigns the transform of the first child of the GameObject this script is attached to.
        //m_BulletSpawnPoint = transform.GetChild(0);
    }


    // Update is called once per frame. It is the main workhorse function for frame updates.
    void Update()
    {
        if(ShipInput.IsShooting())
        {
            //Rigidbody bulletInstance = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, m_BulletSpawnPoint.rotation) as Rigidbody;
            //bulletInstance.velocity = m_BulletVelocity * m_BulletSpawnPoint.up; //(up = y-axis)

            // Get a bullet and initialize it before activating it.
            //m_Bullet = GameManager.instance.m_BulletPool.Pop();
            m_Bullet = m_BulletPool.Pop();
            Rigidbody rigidbody = m_Bullet.GetComponent<Rigidbody>();
            m_Bullet.transform.position = m_BulletSpawnPoint.position;
            m_Bullet.transform.rotation = m_BulletSpawnPoint.rotation;
            rigidbody.velocity = m_BulletVelocity * m_BulletSpawnPoint.up; //(up: y-axis)
            m_Bullet.gameObject.SetActive(true);

            //Invoke("Repool", m_BulletLife);
            //StartCoroutine(Repool(m_Bullet, m_BulletLife));

            // Change the clip to the firing clip and play it.
            //m_ShootingAudio.clip = m_ShootClip;
            m_ShootingAudio.Play();
            //SoundManager.instance.PlaySingle(m_ShootClip);
        }
    }


    // Coroutine to repool an object after a delay.
    //IEnumerator Repool(Poolable p, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    //GameManager.instance.m_BulletPool.Push(p);
    //    m_BulletPool.Push(p);
    //    //Debug.Log("Pooled");
    //}

} // end class
