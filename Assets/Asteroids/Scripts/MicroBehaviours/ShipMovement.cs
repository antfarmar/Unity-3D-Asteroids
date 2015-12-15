using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    public float m_Thrust = 1000f;      // Thrust force.
    public float m_Torque = 200f;       // To turn the ship on z-axis.

    Rigidbody m_Rigidbody;              // Reference used to move the ship.
    float m_MovementInputValue;         // The current value of the movement input.
    float m_TurnInputValue;             // The current value of the turn input.

    [HideInInspector]
    public bool hasThrustPowerup;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
        hasThrustPowerup = false;
    }

    void Update()
    {
        m_TurnInputValue = ShipInput.GetTurnAxis();
        m_MovementInputValue = ShipInput.GetForwardThrust();
    }

    void FixedUpdate()
    {
        Move();
        Turn();
    }


    void Move()
    {
        // Create a vector in the direction the ship is facing.
        // Magnitude based on the input, speed and the time between frames.
        Vector3 thrustForce = transform.up * m_MovementInputValue * Time.deltaTime;
        thrustForce *= hasThrustPowerup ? 2f * m_Thrust : m_Thrust;
        m_Rigidbody.AddForce(thrustForce);
    }

    void Turn()
    {
        // Determine the torque based on the input, force and time between frames.
        float turn = m_TurnInputValue * m_Torque * Time.deltaTime;
        Vector3 zTorque = transform.forward * -turn;
        m_Rigidbody.AddTorque(zTorque);
    }
}
