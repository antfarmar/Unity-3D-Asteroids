using UnityEngine;
using System.Collections;

public class PowerupFire : Powerup
{

    public override void GrantPower()
    {
        ShipShooter shooter = ship.GetComponent<ShipShooter>();
        int weaponChoice = Random.Range(1, (int)ShipShooter.Weapons.Count);
        shooter.activeWeapon = weaponChoice;
        base.GrantPower();
    }

    public override void DenyPower()
    {
        ShipShooter shooter = ship.GetComponent<ShipShooter>();
        shooter.activeWeapon = (int)ShipShooter.Weapons.Default;
        base.DenyPower();
    }
}
