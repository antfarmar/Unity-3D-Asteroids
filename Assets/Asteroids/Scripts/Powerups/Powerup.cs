using UnityEngine;
using System.Collections;
using System;

public class Powerup : GameToken
{
    protected GameObject ship; // Reference obtained on collisions.

    [Range(5, 30)]
    public int showTime = 10;

    [Range(5, 30)]
    public int powerDuration = 10;

    public bool isVisible;

    void Start()
    {
        HideInScene();
    }

    //void Update()
    //{
    //    //animation?
    //}

    protected virtual void OnCollisionEnter(Collision otherCollision)
    {
        GameObject otherObject = otherCollision.gameObject;
        if (otherObject.tag == "Ship")
        {
            Score(destructionScore);
            ship = otherObject;
            HideInScene();
            GrantPower();
        }
    }

    protected virtual void GrantPower()
    {
        CancelInvoke("DenyPower"); // Allows power "refresh" if got again.
        Invoke("DenyPower", powerDuration);
    }

    protected virtual void DenyPower() { }

    public void ShowInScene()
    {
        SetVisibility(true);
        Respawn();
    }

    public void HideInScene()
    {
        SetVisibility(false);
    }

    void Respawn()
    {
        int mask = LayerMask.GetMask("Asteroid");
        float collisionSphereRadius = transform.localScale.x;
        Vector3 position = Spawn.FindSuitableSpawnLocation(mask, collisionSphereRadius);
        SpawnAt(position);
    }

    void SetVisibility(bool isVisible)
    {
        this.isVisible = isVisible;
        gameObject.GetComponent<Renderer>().enabled = this.isVisible;
        gameObject.GetComponent<Collider>().enabled = this.isVisible;
    }
}
