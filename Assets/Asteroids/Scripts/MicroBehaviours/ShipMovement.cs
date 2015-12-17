using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    public float thrust = 1000f;
    public float torque = 200f;
    public AudioSource hyperAudio;

    Rigidbody rbody;
    float thrustInput;
    float turnInput;

    [HideInInspector]
    public bool hasThrustPowerup;

    void Reset()
    {
        thrustInput = 0f;
        turnInput = 0f;
        GetComponent<Ship>().ResetRigidbody();
    }

    void Awake() { rbody = GetComponent<Rigidbody>(); }

    void OnEnable() { Reset(); hasThrustPowerup = false; }

    void Update()
    {
        if (ShipInput.IsHyperspacing()) { HyperSpace(); return; }
        turnInput = ShipInput.GetTurnAxis();
        thrustInput = ShipInput.GetForwardThrust();
    }

    void FixedUpdate() { Move(); Turn(); }

    void HyperSpace()
    {
        Reset();
        transform.position = Viewport.GetRandomWorldPositionXY();
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(1, 360));
        hyperAudio.Play();
    }

    void Move()
    {
        // Create a vector in the direction the ship is facing.
        // Magnitude based on the input, speed and the time between frames.
        Vector3 thrustForce = transform.up * thrustInput * Time.deltaTime;
        thrustForce *= hasThrustPowerup ? 2f * thrust : thrust;
        rbody.AddForce(thrustForce);
    }

    void Turn()
    {
        // Determine the torque based on the input, force and time between frames.
        float turn = turnInput * torque * Time.deltaTime;
        Vector3 zTorque = transform.forward * -turn;
        rbody.AddTorque(zTorque);
    }
}
