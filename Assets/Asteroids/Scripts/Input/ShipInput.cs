using UnityEngine;

public static class ShipInput
{
    public static bool IsShooting()
    {
        return Input.GetButtonDown("Fire1");
    }

    public static bool IsHyperspacing()
    {
        return Input.GetButtonDown("Fire2");
    }

    public static float GetTurnAxis()
    {
        return Input.GetAxis("Horizontal");
    }

    public static float GetForwardThrust()
    {
        float axis = Input.GetAxis("Vertical");
        return Mathf.Clamp01(axis);
    }
}
