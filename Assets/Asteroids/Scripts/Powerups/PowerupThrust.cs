using UnityEngine;
using System.Collections;

public class PowerupThrust : Powerup
{

    protected override void GrantPower()
    {
        ShipMovement mover = ship.GetComponent<ShipMovement>();
        mover.hasThrustPowerup = true;
        base.GrantPower();
    }

    protected override void DenyPower()
    {
        ShipMovement mover = ship.GetComponent<ShipMovement>();
        mover.hasThrustPowerup = false;
        base.DenyPower();
    }
}
