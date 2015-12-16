using UnityEngine;
using System.Collections;

public class PowerupFire : Powerup
{

    protected override void GrantPower()
    {
        ShipShooter shooter = ship.GetComponent<ShipShooter>();
        int weaponChoice = Random.Range(1, (int)ShipShooter.Weapons.Count);
        shooter.activeWeapon = weaponChoice;
        base.GrantPower();
    }

    protected override void DenyPower()
    {
        ShipShooter shooter = ship.GetComponent<ShipShooter>();
        shooter.activeWeapon = (int)ShipShooter.Weapons.Default;
        base.DenyPower();
    }
}
