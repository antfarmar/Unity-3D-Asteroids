using UnityEngine;
using System.Collections;

public class PowerupShield : Powerup
{

    protected override void GrantPower()
    {
        GameObject shield = ship.transform.Find("Shield").gameObject;
        shield.SetActive(true);
        base.GrantPower();
    }

    protected override void DenyPower()
    {
        GameObject shield = ship.transform.Find("Shield").gameObject;
        shield.SetActive(false);
        base.DenyPower();
    }
}
