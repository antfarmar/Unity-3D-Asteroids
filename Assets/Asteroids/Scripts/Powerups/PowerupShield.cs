using UnityEngine;
using System.Collections;

public class PowerupShield : Powerup
{

    public override void GrantPower()
    {
        GameObject shield = ship.transform.Find("Shield").gameObject;
        shield.transform.position = ship.transform.position; // in case it shifted by physics
        shield.SetActive(true);
        base.GrantPower();
    }

    public override void DenyPower()
    {
        GameObject shield = ship.transform.Find("Shield").gameObject;
        shield.SetActive(false);
        base.DenyPower();
    }
}
