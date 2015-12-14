using UnityEngine;
using System.Collections;
using System;

public class Powerup : GameBehaviour
{

    public GameObject prefab;

    protected GameObject ship;

    [Range(5,30)]
    public int lifetime = 10;

    [Range(5,30)]
    public int activeDuration = 5;

    void Start()
    {
        InvokeRemoveFromGame(lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        //animation?
    }

    protected virtual void OnCollisionEnter(Collision otherCollision)
    {
        GameObject otherObject =  otherCollision.gameObject;
        if (otherObject.tag == "Ship")
        {
            ship = otherObject;
            CancelInvoke("RemoveFromGame");
            HidePowerup();
            GrantPowerup();
        }
    }

    protected virtual void GrantPowerup()
    {
        Invoke("RemovePowerup", activeDuration);
    }

    protected virtual void RemovePowerup()
    {
        Debug.Log("REMOVE BASE POWERUP");
        RemoveFromGame();
    }



    void HidePowerup()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
    }
}
