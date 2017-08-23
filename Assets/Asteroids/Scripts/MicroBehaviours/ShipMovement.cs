using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    public float thrust = 1000f;
    public float torque = 500f;
    public float maxSpeed = 20f;
    public float powerupThrust = 2000f;  
    public float powerupSpeed = 30f;  
    public AudioSource hyperAudio;

    Rigidbody rb;
    float thrustInput;
    float turnInput;

    [HideInInspector]
    public bool hasThrustPowerup;

    void Reset()
    {
        thrustInput = 0f;
        turnInput = 0f;
    }

    void Awake() { rb = GetComponent<Rigidbody>(); }

    void OnEnable() { Reset(); hasThrustPowerup = false; }

    void Update()
    {
        if (ShipInput.IsHyperspacing()) { HyperSpace(); return; }
        turnInput = ShipInput.GetTurnAxis();
        thrustInput = ShipInput.GetForwardThrust();
    }

    void FixedUpdate() { Move(); Turn(); ClampSpeed(); }

    void ClampSpeed()
    {
        float clampSpeed = hasThrustPowerup ? powerupSpeed : maxSpeed;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, clampSpeed);
    }

    void HyperSpace()
    {
        RigidbodyExt.Reset(rb);
        transform.position = Viewport.GetRandomWorldPositionXY();
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(1, 360));
        hyperAudio.Play();
    }

    void Move()
    {
        // Quick hack to have powerup affect thrust.
        float useThrust = hasThrustPowerup ? powerupThrust : thrust;

        // Create a vector in the direction the ship is facing.
        // Magnitude based on the input, speed and the time between frames.
        Vector3 thrustForce = thrustInput * useThrust * Time.deltaTime * transform.up;
        rb.AddForce(thrustForce);
    }

    void Turn()
    {
        // Determine the torque based on the input, force and time between frames.
        float turn = turnInput * torque * Time.deltaTime;
        Vector3 zTorque = transform.forward * -turn;
        rb.AddTorque(zTorque);
    }
}
