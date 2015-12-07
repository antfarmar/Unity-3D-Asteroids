using UnityEngine;

public static class ShipInput
{
    public static bool IsShooting()
    {
<<<<<<< HEAD
        return Input.GetButtonDown("Fire1");
=======
        return Input.GetButtonDown("Fire3");
>>>>>>> refs/heads/pr/27
    }

    public static float GetTurnAxis()
    {
        return Input.GetAxis("Horizontal");
    }

    public static float GetForwardThrust()
    {
        float axis = Input.GetAxis("Vertical");
<<<<<<< HEAD
        return Mathf.Clamp01(axis); // No backpedal
    }
}
=======
        return Mathf.Clamp01(axis);
    }
}
>>>>>>> refs/heads/pr/27
