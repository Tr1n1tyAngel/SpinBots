using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotSelection : MonoBehaviour
{
    public GameManager gameManager;
    private MinimaxAI ai;
    private GameState gameState;

    private void Start()
    {
        ai = new MinimaxAI();
        gameState = new GameState
        {
            p1AttackStatAI = gameManager.p1AttackStat,
            p1DefenseStatAI = gameManager.p1DefenseStat,
            p1StaminaStatAI = gameManager.p1StaminaStat,
            p2AttackStatAI = gameManager.p2AttackStat,
            p2DefenseStatAI = gameManager.p2DefenseStat,
            p2StaminaStatAI = gameManager.p2StaminaStat,
            p1BotSelectedAI = gameManager.p1BotSelected,
            p2BotSelectedAI = gameManager.p2BotSelected,
            p1ReadyAI = gameManager.p1Ready,
            p2ReadyAI = gameManager.p2Ready,
            roundCountAI = gameManager.roundCount,
            bgRoundCountAI = gameManager.bgRoundCount,
            player1ScoreAI = gameManager.player1Score,
            player2ScoreAI = gameManager.player2Score,
            gameOverAI = false
        };
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && gameManager.p1AttackType == false)
        {
            gameManager.p1AttackType = true;
            gameManager.p1DefenseType = false;
            gameManager.p1StaminaType = false;
            gameManager.p1BotSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.S) && gameManager.p1DefenseType == false)
        {
            gameManager.p1AttackType = false;
            gameManager.p1DefenseType = true;
            gameManager.p1StaminaType = false;
            gameManager.p1BotSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.D) && gameManager.p1StaminaType == false)
        {
            gameManager.p1AttackType = false;
            gameManager.p1DefenseType = false;
            gameManager.p1StaminaType = true;
            gameManager.p1BotSelected = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && gameManager.p1BotSelected)
        {
            gameManager.p1Ready = true;
            gameState.p1ReadyAI = true;
        }

        if (SceneManager.GetActiveScene().name == "PVP")
        {
            if (Input.GetKeyDown(KeyCode.J) && gameManager.p2AttackType == false)
            {
                gameManager.p2AttackType = true;
                gameManager.p2DefenseType = false;
                gameManager.p2StaminaType = false;
                gameManager.p2BotSelected = true;
            }
            if (Input.GetKeyDown(KeyCode.K) && gameManager.p2DefenseType == false)
            {
                gameManager.p2AttackType = false;
                gameManager.p2DefenseType = true;
                gameManager.p2StaminaType = false;
                gameManager.p2BotSelected = true;
            }
            if (Input.GetKeyDown(KeyCode.L) && gameManager.p2StaminaType == false)
            {
                gameManager.p2AttackType = false;
                gameManager.p2DefenseType = false;
                gameManager.p2StaminaType = true;
                gameManager.p2BotSelected = true;
            }
            if (Input.GetKeyDown(KeyCode.O) && gameManager.p2BotSelected)
            {
                gameManager.p2Ready = true;
                gameState.p2ReadyAI = true;
            }
        }
        else
        {

            if (!gameManager.p2Ready)
            {
                Move bestMove = ai.GetBestMove(gameState);
                switch (bestMove.Choice)
                {
                    case "Attack":
                        gameManager.p2AttackType = true;
                        gameManager.p2DefenseType = false;
                        gameManager.p2StaminaType = false;
                        break;
                    case "Defense":
                        gameManager.p2AttackType = false;
                        gameManager.p2DefenseType = true;
                        gameManager.p2StaminaType = false;
                        break;
                    case "Stamina":
                        gameManager.p2AttackType = false;
                        gameManager.p2DefenseType = false;
                        gameManager.p2StaminaType = true;
                        break;
                }
                gameManager.p2BotSelected = true;
                gameManager.p2Ready = true;
                gameState.p2ReadyAI = true;
            }





        }
    }
}
