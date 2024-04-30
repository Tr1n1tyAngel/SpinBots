using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpinBotController : MonoBehaviour
{
    public float initialSpinForce = 500f;  // The initial force to spin
    public float movementSpeed = 5f;  // The speed of the movement across the arena
    public float curveMagnitude = 1f;

    private Rigidbody rb;
    private float currentSpinForce;
    private float currentMovementSpeed;
    private int totalSteps = 18000; // Total steps to stop based on 6 minutes and FixedUpdate rate of 0.02
    private float spinDecayPerStep;
    private float movementDecayPerStep;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = Mathf.Infinity;
        currentSpinForce = initialSpinForce;
        currentMovementSpeed = movementSpeed;

        // Calculate decay per step to reach zero after totalSteps
        spinDecayPerStep = initialSpinForce / totalSteps;
        movementDecayPerStep = movementSpeed / totalSteps;

        Spin();
        InitialMovement();
    }

    void Spin()
    {
        rb.AddTorque(transform.up * currentSpinForce, ForceMode.Force);
    }

    void InitialMovement()
    {
        Vector3 initialDirection = (transform.forward + transform.right * Random.Range(-0.5f, 0.5f)).normalized;
        rb.AddForce(initialDirection * currentMovementSpeed, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        // Reduce spin force and movement speed over time
        if (currentSpinForce > 0)
        {
            currentSpinForce -= spinDecayPerStep;
            rb.AddTorque(transform.up * currentSpinForce, ForceMode.Force);
        }

        if (currentMovementSpeed > 0)
        {
            currentMovementSpeed -= movementDecayPerStep;
            Vector3 curveForce = Vector3.Cross(rb.velocity, transform.up) * curveMagnitude;
            rb.velocity = rb.velocity.normalized * currentMovementSpeed;
            rb.AddForce(curveForce, ForceMode.Force);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("SpinBot collided with: " + collision.gameObject.name);

        // Reflect the Beyblade's velocity on collision
        if (collision.gameObject.CompareTag("SpinBot") || collision.gameObject.CompareTag("ArenaWall"))
        {
            Vector3 normal = collision.contacts[0].normal;
            normal.y = 0;
            Vector3 reflect = Vector3.Reflect(rb.velocity, normal);
            rb.velocity = reflect.normalized * currentMovementSpeed;
        }
    }
}
