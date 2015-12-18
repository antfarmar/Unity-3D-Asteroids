using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    public float thrust = 1000f;
    public float torque = 400f;
    public float maxSpeed = 10f;
    public float powerupSpeed = 20f;
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
        // Create a vector in the direction the ship is facing.
        // Magnitude based on the input, speed and the time between frames.
        Vector3 thrustForce = transform.up * thrustInput * thrust * Time.deltaTime;
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
