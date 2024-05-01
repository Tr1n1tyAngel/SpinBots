using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class P2SpinBotController : MonoBehaviour
{
    public GameManager gameManager;

    public float initialSpinForce = 500f;  // The initial force to spin
    public float movementSpeed = 5f;  // The speed of the movement across the arena
    public float curveMagnitude = 1f;

    private Rigidbody rb;
    private float currentSpinForce;
    private float currentMovementSpeed;
    private int totalSteps = 18000; // Total steps to stop based on 6 minutes and FixedUpdate rate of 0.02

    private float spinDecayPerStep;
    private float movementDecayPerStep;

    private float lastAttackStat;
    private float lastDefenseStat;

    void Start()
    {
        

        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = Mathf.Infinity;
        currentSpinForce = initialSpinForce;
        currentMovementSpeed = movementSpeed;

        // Calculate decay per step to reach zero after totalSteps
        spinDecayPerStep = initialSpinForce / totalSteps;
        movementDecayPerStep = movementSpeed / totalSteps;

        InitializeBotStats();
        Spin();
        InitialMovement();
    }

    void InitializeBotStats()
    {
        lastAttackStat = gameManager.p1AttackStat;
        lastDefenseStat = gameManager.p1DefenseStat;

        UpdateStats(true);
    }
    void UpdateBotStats()
    {
        UpdateStats(false); // Regular update with false

        // Apply forces if not stopped
        if (currentSpinForce > 0)
        {
            rb.AddTorque(transform.up * currentSpinForce, ForceMode.Force);
        }
        if (currentMovementSpeed > 0)
        {
            ApplyForces();
        }
    }
    void UpdateStats(bool isInitial)
    {
        float attackChange = gameManager.p2AttackStat - lastAttackStat;
        float defenseChange = gameManager.p2DefenseStat - lastDefenseStat;
        float staminaModifier = gameManager.p2StaminaStat / 10.0f;

        if (!isInitial)
        {
            // Apply incremental changes
            
            currentMovementSpeed += attackChange * 0.1f - defenseChange * 0.1f;
        }
        else
        {
            
            // Initial setting of forces

            currentMovementSpeed = movementSpeed + (gameManager.p2AttackStat - gameManager.p2DefenseStat) * 0.1f;
        }

        lastAttackStat = gameManager.p2AttackStat;
        lastDefenseStat = gameManager.p2DefenseStat;

        // Update decay steps based on total gameplay duration
        spinDecayPerStep = currentSpinForce / (17750 + staminaModifier);
        movementDecayPerStep = currentMovementSpeed / 16750;
    }
    void Spin()
    {
        rb.AddTorque(-transform.up * currentSpinForce, ForceMode.Force);
    }

    void InitialMovement()
    {
        Vector3 initialDirection = -(transform.forward + transform.right * Random.Range(-0.5f, 0.5f)).normalized;
        rb.AddForce(initialDirection * currentMovementSpeed, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        UpdateBotStats();
        UpdateDecayRates();
    }

    void UpdateDecayRates()
    {
        if (gameManager.battleTimer <= 15)
        {
            rb.angularVelocity = Vector3.zero;  // Explicitly stop spinning
            currentSpinForce = 0;  // Set spin force to zero
        }
        if (gameManager.battleTimer <= 25)
        {
            rb.velocity = Vector3.zero;  // Explicitly stop moving
            currentMovementSpeed = 0;  // Set movement speed to zero
        }

        // Reduce spin force and movement speed over time if not already stopped
        if (currentSpinForce > 0)
        {
            currentSpinForce -= spinDecayPerStep;
        }
        else if (currentSpinForce < 0)
        {
            currentSpinForce = 0;
        }

        if (currentMovementSpeed > 0)
        {
            currentMovementSpeed -= movementDecayPerStep;
            ApplyForces();
        }
    }
    public void ApplyForces()
    {
        Vector3 curveForce =Vector3.Cross(rb.velocity, transform.up) * curveMagnitude;
        rb.velocity = rb.velocity.normalized * currentMovementSpeed;
        rb.AddForce(curveForce, ForceMode.Force);
    }
    private void Update()
    {
        if (gameManager.p2AttackType)
        {
            curveMagnitude = 5;
        }
        else if (gameManager.p2DefenseType)
        {
            curveMagnitude = 3;
        }
        else if (gameManager.p2StaminaType)
        {
            curveMagnitude = 7;
        }
        Debug.Log(currentMovementSpeed);
        Debug.Log(currentSpinForce);
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
        if (collision.gameObject.CompareTag("ArenaWall"))
        {
            // Decrease stats slightly upon hitting a wall
            gameManager.p2AttackStat -= 0.001f;
            gameManager.p2DefenseStat -= 0.001f;
            gameManager.p2StaminaStat -= 0.001f;
        }
        if (collision.gameObject.CompareTag("SpinBot"))
        {
            // Stat comparison and adjustment
            float attackDifference = gameManager.p2AttackStat - gameManager.p1AttackStat;
            float defenseDifference = gameManager.p2DefenseStat - gameManager.p1DefenseStat;
            float staminaDifference = gameManager.p2StaminaStat - gameManager.p1StaminaStat;

            // Calculate the stat decrease
            if (attackDifference > 0) { gameManager.p2StaminaStat -= attackDifference *0.1f; }
            if (defenseDifference > 0) { gameManager.p2AttackStat -= defenseDifference * 0.1f; }
            if (staminaDifference > 0) { gameManager.p2DefenseStat -= staminaDifference * 0.1f; }
        }
    }
   
}
