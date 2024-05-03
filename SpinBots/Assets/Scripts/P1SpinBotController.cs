using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class P1SpinBotController : MonoBehaviour
{
    public GameManager gameManager;
    
    
    void Start()
    {
      

        gameManager.p1Rigidbody = GetComponent<Rigidbody>();
        gameManager.p1Rigidbody.maxAngularVelocity = Mathf.Infinity;
        gameManager.p1CurrentSpinForce = gameManager.p1InitialSpinForce;
        gameManager.p1CurrentMovementSpeed = gameManager.p1MovementSpeed;

        // Calculate decay per step to reach zero after totalSteps
        

        InitializeBotStats();
        Spin();
        InitialMovement();
    }

    void InitializeBotStats()
    {
        gameManager.p1LastAttackStat = gameManager.p1AttackStat;
        gameManager.p1LastDefenseStat = gameManager.p1DefenseStat;

        UpdateStats(true);
    }
    void UpdateBotStats()
    {
        UpdateStats(false); // Regular update with false

        // Apply forces if not stopped
        if (gameManager.p1CurrentSpinForce > 0)
        {
            gameManager.p1Rigidbody.AddTorque(transform.up * gameManager.p1CurrentSpinForce, ForceMode.Force);
        }
        if (gameManager.p1CurrentMovementSpeed > 0)
        {
            ApplyForces();
        }
    }
    void UpdateStats(bool isInitial)
    {
        float attackChange = gameManager.p1AttackStat - gameManager.p1LastAttackStat;
        float defenseChange = gameManager.p1DefenseStat - gameManager.p1LastDefenseStat;
        float staminaModifier = gameManager.p1StaminaStat / 10.0f;

        if (!isInitial)
        {
            // Apply incremental changes

            gameManager.p1CurrentMovementSpeed += attackChange * 0.1f - defenseChange * 0.1f;
        }
        else
        {


            // Initial setting of forces

            gameManager.p1CurrentMovementSpeed = gameManager.p1MovementSpeed + (gameManager.p1AttackStat - gameManager.p1DefenseStat) * 0.1f;
        }

        gameManager.p1LastAttackStat = gameManager.p1AttackStat;
        gameManager.p1LastDefenseStat = gameManager.p1DefenseStat;

        // Update decay steps based on total gameplay duration
        gameManager.p1SpinDecayPerStep = gameManager.p1CurrentSpinForce / (17750 + staminaModifier);
        gameManager.p1MovementDecayPerStep = gameManager.p1CurrentMovementSpeed / 16750;
    }
    void Spin()
    {
        gameManager.p1Rigidbody.AddTorque(transform.up * gameManager.p1CurrentSpinForce, ForceMode.Force);
    }

    void InitialMovement()
    {
        Vector3 initialDirection = (transform.forward + transform.right * Random.Range(-0.5f, 0.5f)).normalized;
        gameManager.p1Rigidbody.AddForce(initialDirection * gameManager.p1CurrentMovementSpeed, ForceMode.Impulse);
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
            gameManager.p1Rigidbody.angularVelocity = Vector3.zero;  // Explicitly stop spinning
            gameManager.p1CurrentSpinForce = 0;  // Set spin force to zero
            gameManager.p1Rigidbody.velocity = Vector3.zero;  // Explicitly stop moving
            gameManager.p1CurrentMovementSpeed = 0;  // Set movement speed to zero
        }
        // Reduce spin force and movement speed over time if not already stopped
        if (gameManager.p1CurrentSpinForce > 0)
        {
            gameManager.p1CurrentSpinForce -= gameManager.p1SpinDecayPerStep;
            
        }
        else if (gameManager.p1CurrentSpinForce < 0)
        {
            gameManager.p1CurrentSpinForce = 0;
        }

        if (gameManager.p1CurrentMovementSpeed > 0)
        {
            gameManager.p1CurrentMovementSpeed -= gameManager.p1MovementDecayPerStep;
            ApplyForces();
        }
    }
    public void ApplyForces()
    {
        if(!gameManager.p1Charge || gameManager.isP1Grounded)
        {
            Vector3 curveForce = Vector3.Cross(gameManager.p1Rigidbody.velocity, transform.up) * gameManager.p1CurveMagnitude;
            gameManager.p1Rigidbody.velocity = gameManager.p1Rigidbody.velocity.normalized * gameManager.p1CurrentMovementSpeed;
            gameManager.p1Rigidbody.AddForce(curveForce, ForceMode.Force);
        }
       
    }
    private void Update()
    {
        if (gameManager.p1AttackType)
        {
            gameManager.p1CurveMagnitude = 4;
        }
        else if (gameManager.p1DefenseType)
        {
            gameManager.p1CurveMagnitude = 3;
        }
        else if (gameManager.p1StaminaType)
        {
            gameManager.p1CurveMagnitude = 5;
        }
        
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("SpinBot collided with: " + collision.gameObject.name);
        if(collision.gameObject.CompareTag("Ground"))
        {
            gameManager.isP1Grounded = true;
        }
        if(collision.gameObject.CompareTag("OutOfBounds"))
        {
            gameManager.finalWinnerResultTxt.text = "Player 1 was knocked out of bounds player 2 wins";
            gameManager.EndGame();

        }
        
        // Reflect the Beyblade's velocity on collision
        if (collision.gameObject.CompareTag("P2SpinBot"))
        {
            Vector3 normal = collision.contacts[0].normal;
            normal.y = 0;
            Vector3 reflect = Vector3.Reflect(gameManager.p1Rigidbody.velocity, normal);
            gameManager.p1Rigidbody.velocity = reflect.normalized * gameManager.p1CurrentMovementSpeed;
        }

        if(!gameManager.p1Shield)
        {
            if (collision.gameObject.CompareTag("ArenaWall"))
            {
                // Decrease stats slightly upon hitting a wall
                gameManager.p1AttackStat -= 0.05f;
                gameManager.p1DefenseStat -= 0.05f;
                gameManager.p1StaminaStat -= 0.05f;

                gameManager.p1Charge = false;
            }
            if (collision.gameObject.CompareTag("P2SpinBot"))
            {
                // Stat comparison and adjustment
                float attackDifference = gameManager.p1AttackStat - gameManager.p2AttackStat;
                float defenseDifference = gameManager.p1DefenseStat - gameManager.p2DefenseStat;
                float staminaDifference = gameManager.p1StaminaStat - gameManager.p2StaminaStat;

                // Calculate the stat decrease
                if (attackDifference > 0) { gameManager.p1StaminaStat -= attackDifference * 0.1f; }
                if (defenseDifference > 0) { gameManager.p1AttackStat -= defenseDifference * 0.1f; }
                if (staminaDifference > 0) { gameManager.p1DefenseStat -= staminaDifference * 0.1f; }

                if (gameManager.p1Charge)
                {
                    gameManager.p2AttackStat -= 0.5f;
                    gameManager.p2DefenseStat -= 0.5f;
                    gameManager.p2StaminaStat -= 0.5f;
                    gameManager.p1Charge = false;
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
