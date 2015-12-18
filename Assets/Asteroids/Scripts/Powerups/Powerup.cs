using UnityEngine;
using System.Collections;
using System;

public class Powerup : GameToken
{
    protected GameObject ship; // receiver of powerups
    public void SetReceiver(GameObject receiver) { ship = receiver; }

    [Range(5, 30)]
    public int showTime = 10;

    [Range(5, 30)]
    public int powerDuration = 10;

    public bool isVisible;

    void Start() { HideInScene(); }

    //void Update()
    //{
    //    //animation?
    //}

    protected virtual void OnCollisionEnter(Collision otherCollision)
    {
        GameObject otherObject = otherCollision.gameObject;
        if (otherObject.tag == ship.tag)
        {
            Score(destructionScore);
            HideInScene();
            GrantPower();
        }
    }

    public virtual void GrantPower()
    {
        CancelInvoke("DenyPower"); // Allows power "refresh" if got again.
        Invoke("DenyPower", powerDuration);
    }

    public virtual void DenyPower() { }

    public void ShowInScene()
    {
        Invoke("HideInScene", showTime);
        SetVisibility(true);
        Respawn();
    }

    public void HideInScene()
    {
        CancelInvoke("HideInScene");
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
