using UnityEngine;
using System.Collections;

public class PowerupShield : Powerup
{

    protected override void GrantPowerup()
    {
        GameObject shield = ship.transform.Find("Shield").gameObject;
        shield.SetActive(true);
        base.GrantPowerup();
    }

    protected override void RemovePowerup()
    {
        Debug.Log("REMOVE POWERUP");
        GameObject shield = ship.transform.Find("Shield").gameObject;
        shield.SetActive(false);
        base.RemovePowerup();
    }
}
