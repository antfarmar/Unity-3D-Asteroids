using UnityEngine;
using System.Collections;

public class PowerupShield : Powerup
{
    public override void GrantPower()
    {
        ShieldBehaviour shield = GetShield();
        shield.activateShield(powerDuration);
        base.GrantPower();
    }

    public override void DenyPower()
    {
        ShieldBehaviour shield = GetShield();
        shield.deactivateShield();
        base.DenyPower();
    }

    ShieldBehaviour GetShield()
    {
        GameObject shield = ship.transform.Find("Shield").gameObject;
        return shield.GetComponentsInChildren<ShieldBehaviour>(true)[0]; // in case ship deactivated.
    }

    //public override void GrantPower()
    //{
    //    GameObject shield = ship.transform.Find("Shield").gameObject;
    //    shield.transform.position = ship.transform.position; // in case it shifted by physics
    //    shield.SetActive(true);
    //    base.GrantPower();
    //}

    //public override void DenyPower()
    //{
    //    GameObject shield = ship.transform.Find("Shield").gameObject;
    //    shield.SetActive(false);
    //    base.DenyPower();
    //}
}
