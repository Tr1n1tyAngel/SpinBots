using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class P2SpinBotController : MonoBehaviour
{
    public GameManager gameManager;

    
    

    void Start()
    {
        

        gameManager.p2Rigidbody = GetComponent<Rigidbody>();
        gameManager.p2Rigidbody.maxAngularVelocity = Mathf.Infinity;
        gameManager.p2CurrentSpinForce = gameManager.p2InitialSpinForce;
        gameManager.p2CurrentMovementSpeed = gameManager.p2MovementSpeed;

        // Calculate decay per step to reach zero after totalSteps
        

        InitializeBotStats();
        Spin();
        InitialMovement();
    }

    void InitializeBotStats()
    {
        gameManager.p2LastAttackStat = gameManager.p1AttackStat;
        gameManager.p2LastDefenseStat = gameManager.p1DefenseStat;

        UpdateStats(true);
    }
    void UpdateBotStats()
    {
        UpdateStats(false); // Regular update with false

        // Apply forces if not stopped
        if (gameManager.p2CurrentSpinForce > 0)
        {
            gameManager.p2Rigidbody.AddTorque(-transform.up * gameManager.p2CurrentSpinForce, ForceMode.Force);
        }
        if (gameManager.p2CurrentMovementSpeed > 0)
        {
            ApplyForces();
        }
    }
    void UpdateStats(bool isInitial)
    {
        float attackChange = gameManager.p2AttackStat - gameManager.p2LastAttackStat;
        float defenseChange = gameManager.p2DefenseStat - gameManager.p2LastDefenseStat;
        float staminaModifier = gameManager.p2StaminaStat / 10.0f;

        if (!isInitial)
        {
            // Apply incremental changes

            gameManager.p2CurrentMovementSpeed += attackChange * 0.1f - defenseChange * 0.1f;
        }
        else
        {

            // Initial setting of forces

            gameManager.p2CurrentMovementSpeed = gameManager.p2MovementSpeed + (gameManager.p2AttackStat - gameManager.p2DefenseStat) * 0.1f;
        }

        gameManager.p2LastAttackStat = gameManager.p2AttackStat;
        gameManager.p2LastDefenseStat = gameManager.p2DefenseStat;

        // Update decay steps based on total gameplay duration
        gameManager.p2SpinDecayPerStep = gameManager.p2CurrentSpinForce / (17750 + staminaModifier);
        gameManager.p2MovementDecayPerStep = gameManager.p2CurrentMovementSpeed / 16750;
    }
    void Spin()
    {
        gameManager.p2Rigidbody.AddTorque(-transform.up * gameManager.p2CurrentSpinForce, ForceMode.Force);
    }

    void InitialMovement()
    {
        Vector3 initialDirection = -(transform.forward + transform.right * Random.Range(-0.5f, 0.5f)).normalized;
        gameManager.p2Rigidbody.AddForce(initialDirection * gameManager.p2CurrentMovementSpeed, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        UpdateBotStats();
        UpdateDecayRates();
    }

    void UpdateDecayRates()
    {
        if (gameManager.battleTimer <= 0)
        {
            gameManager.p2Rigidbody.angularVelocity = Vector3.zero;  // Explicitly stop spinning
            gameManager.p2CurrentSpinForce = 0;  // Set spin force to zero
            gameManager.p2Rigidbody.velocity = Vector3.zero;  // Explicitly stop moving
            gameManager.p2CurrentMovementSpeed = 0;  // Set movement speed to zero
        }

        // Reduce spin force and movement speed over time if not already stopped
        if (gameManager.p2CurrentSpinForce > 0)
        {
            gameManager.p2CurrentSpinForce -= gameManager.p2SpinDecayPerStep;
        }
        else if (gameManager.p2CurrentSpinForce < 0)
        {
            gameManager.p2CurrentSpinForce = 0;
        }

        if (gameManager.p2CurrentMovementSpeed > 0)
        {
            gameManager.p2CurrentMovementSpeed -= gameManager.p2MovementDecayPerStep;
            ApplyForces();
        }
    }
    public void ApplyForces()
    {
        if(!gameManager.p2Charge || gameManager.isP2Grounded)
        {
            Vector3 curveForce = Vector3.Cross(gameManager.p2Rigidbody.velocity, -transform.up) * gameManager.p2CurveMagnitude;
            gameManager.p2Rigidbody.velocity = gameManager.p2Rigidbody.velocity.normalized * gameManager.p2CurrentMovementSpeed;
            gameManager.p2Rigidbody.AddForce(curveForce, ForceMode.Force);
        }
        
    }
    private void Update()
    {
        if (gameManager.p2AttackType)
        {
            gameManager.p2CurveMagnitude = 4;
        }
        else if (gameManager.p2DefenseType)
        {
            gameManager.p2CurveMagnitude = 3;
        }
        else if (gameManager.p2StaminaType)
        {
            gameManager.p2CurveMagnitude = 5;
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("SpinBot collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground"))
        {
            gameManager.isP2Grounded = true;
        }
        else
        {
            gameManager.isP2Grounded = false;
        }
        if (collision.gameObject.CompareTag("OutOfBounds"))
        {
            gameManager.finalWinnerResultTxt.text = "Player 2 was knocked out of bounds player 1 wins";
            gameManager.EndGame();

        }
        // Reflect the Beyblade's velocity on collision
        if (collision.gameObject.CompareTag("P1SpinBot"))
        {
            Vector3 normal = collision.contacts[0].normal;
            normal.y = 0;
            Vector3 reflect = Vector3.Reflect(gameManager.p2Rigidbody.velocity, normal);
            gameManager.p2Rigidbody.velocity = reflect.normalized * gameManager.p2CurrentMovementSpeed;
        }
        if(!gameManager.p2Shield)
        {
            if (collision.gameObject.CompareTag("ArenaWall"))
            {
                // Decrease stats slightly upon hitting a wall
                gameManager.p2AttackStat -= 0.001f;
                gameManager.p2DefenseStat -= 0.001f;
                gameManager.p2StaminaStat -= 0.001f;

                gameManager.p2Charge = false;
            }
            if (collision.gameObject.CompareTag("P1SpinBot"))
            {
                // Stat comparison and adjustment
                float attackDifference = gameManager.p2AttackStat - gameManager.p1AttackStat;
                float defenseDifference = gameManager.p2DefenseStat - gameManager.p1DefenseStat;
                float staminaDifference = gameManager.p2StaminaStat - gameManager.p1StaminaStat;

                // Calculate the stat decrease
                if (attackDifference > 0) { gameManager.p2StaminaStat -= attackDifference * 0.1f; }
                if (defenseDifference > 0) { gameManager.p2AttackStat -= defenseDifference * 0.1f; }
                if (staminaDifference > 0) { gameManager.p2DefenseStat -= staminaDifference * 0.1f; }
                if (gameManager.p2Charge)
                {
                    gameManager.p1AttackStat -= 0.5f;
                    gameManager.p1DefenseStat -= 0.5f;
                    gameManager.p1StaminaStat -= 0.5f;
                    gameManager.p2Charge = false;
                }
            }
        }
        
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameManager.isP1Grounded = false;
        }
    }
   
}
