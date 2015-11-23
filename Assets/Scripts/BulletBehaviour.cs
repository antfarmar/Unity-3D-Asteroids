using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{

    public float m_BulletLife = 1f;
    public ShipShooter m_ShipShooter;


    // OnEnable is called:
    // just after the Behaviour is enabled && GO active.
    // when a MonoBehaviour instance is created.
    // when a GO with the script component is instantiated.
    // when scripts are reloaded after compilation (OnDisable->OnEnable).
    // USAGE: restarting behaviours which are frequently disabled/deactivated.
    void OnEnable()
    {
        Invoke("Repool", m_BulletLife);
    }


    void Repool()
    {
        m_ShipShooter.m_BulletPool.Push(GetComponent<Poolable>());
    }


    // OnDisable is called:
    // when the Behaviour becomes disabled || GO inactive.
    // when the GO is destroyed.
    // when scripts are reloaded after compilation (OnDisable->OnEnable)
    // USAGE: cleanup.
    //void OnDisable() {}


    // Awake is called:
    // exactly once in the lifetime of the script (like Start).
    // upon GO activation or if a fn in any script attached to it is called.
    // even if the script is disabled
    // before any Start functions 
    // after all GO's are initialized
    // USAGE: initialize any references, variables, or game state before the game starts.
    //void Awake() {}


    // Start is called:
    // exactly once in the lifetime of the script (like Awake).
    // only if the script instance is enabled.
    // after all GO's Awake functions have been called (except during gameplay, naturally).
    // on the frame when a script is enabled just before any Update methods called.
    // USAGE: initialization, using refs/vars initialized in Awake.
    //void Start() { }


    // Update is called:
    // only if the MonoBehaviour is enabled.
    // once per frame.
    // USAGE: get Input, most game behaviour.
    //void Update() { }



} // end class

