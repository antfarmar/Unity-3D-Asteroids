using UnityEngine;
using System.Collections;

public class PowerupFire : Powerup
{

    protected override void GrantPowerup()
    {
        ShipShooter shooter = ship.GetComponent<ShipShooter>();
        int weaponChoice = Random.Range(1, (int)ShipShooter.Weapons.Count);
        shooter.currentWeapon = weaponChoice;
        base.GrantPowerup();
    }

    protected override void RemovePowerup()
    {
        Debug.Log("REMOVE POWERUP");
        ShipShooter shooter = ship.GetComponent<ShipShooter>();
        shooter.currentWeapon = (int)ShipShooter.Weapons.Default;
        base.RemovePowerup();
    }
}
