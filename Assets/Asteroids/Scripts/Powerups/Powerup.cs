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
        if (otherObject == ship)
        {
            Score(destructionScore);
            RemoveFromGame();
            GrantPower();
        }
    }

    public virtual void GrantPower()
    {
        CancelInvoke("DenyPower"); // Allows power "refresh" if got again.
        Invoke("DenyPower", powerDuration);
    }

    public virtual void DenyPower() { }

    public void ActivateTemporarily()
    {
        ShowInScene();
        InvokeRemoveFromGame(showTime);
    }

    void ShowInScene()
    {
        Spawn();
        SetVisibility(true);
    }

    void HideInScene()
    {
        CancelInvokeRemoveFromGame();
        SetVisibility(false);
    }

    protected override void RequestDestruction()
    {
        HideInScene();
    }

    void SetVisibility(bool isVisible)
    {
        this.isVisible = isVisible;
        gameObject.GetComponent<Renderer>().enabled = this.isVisible;
        gameObject.GetComponent<Collider>().enabled = this.isVisible;
    }
}
