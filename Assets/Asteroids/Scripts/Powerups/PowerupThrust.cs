using UnityEngine;
using System.Collections;

public class PowerupThrust : Powerup
{

    public override void GrantPower()
    {
        ShipMovement mover = ship.GetComponent<ShipMovement>();
        mover.hasThrustPowerup = true;
        base.GrantPower();
    }

    public override void DenyPower()
    {
        ShipMovement mover = ship.GetComponent<ShipMovement>();
        mover.hasThrustPowerup = false;
        base.DenyPower();
    }
}
