using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuffGame : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager, assign in Unity Editor
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
            p1ChoiceAI = gameManager.player1Choice,
            p2ChoiceAI = gameManager.player2Choice,
            p1ReadyAI = gameManager.p1Ready,
            p2ReadyAI = gameManager.p2Ready,
            roundCountAI = gameManager.roundCount,
            bgRoundCountAI = gameManager.bgRoundCount,
            player1ScoreAI = gameManager.player1Score,
            player2ScoreAI = gameManager.player2Score,
            gameOverAI = false
        };
        RandomizeChoices();
    }

    private void Update()
    {
        Player1Input();
        if (SceneManager.GetActiveScene().name == "PVP")
        {
            Player2Input();
        }
        else 
        {
            AIAutomaticInput();
        }
    }

    private void Player1Input()
    {
        if (Input.GetKeyDown(KeyCode.A)) { gameManager.player1Choice = "Attack"; }
        if (Input.GetKeyDown(KeyCode.S)) { gameManager.player1Choice = "Defense"; }
        if (Input.GetKeyDown(KeyCode.D)) { gameManager.player1Choice = "Stamina"; }

        if (Input.GetKeyDown(KeyCode.E))
        {
            gameManager.p1Ready = true;
            gameState.p1ReadyAI = true;
            Debug.Log("Player 1 chooses " + gameManager.player1Choice);
            CheckPlayersReady();
        }
    }

    private void Player2Input()
    {
        if (Input.GetKeyDown(KeyCode.J)) { gameManager.player2Choice = "Attack"; }
        if (Input.GetKeyDown(KeyCode.K)) { gameManager.player2Choice = "Defense"; }
        if (Input.GetKeyDown(KeyCode.L)) { gameManager.player2Choice = "Stamina"; }

        if (Input.GetKeyDown(KeyCode.O))
        {
            gameManager.p2Ready = true;
            gameState.p2ReadyAI = true;
            Debug.Log("Player 2 chooses " + gameManager.player2Choice);
            CheckPlayersReady();
        }
    }

    private void CheckPlayersReady()
    {
        if (gameManager.p1Ready && gameManager.p2Ready)
        {
            EvaluateRound();
        }
    }

    private void EvaluateRound()
    {
        if (gameManager.player1Choice != gameManager.player2Choice)
        {
            gameManager.IncreaseStat("Player1", gameManager.player1Choice);
            gameManager.IncreaseStat("Player2", gameManager.player2Choice);
            gameManager.bgP1BuffList= gameManager.bgP1BuffList + "\n" + gameManager.player1Choice;
            gameManager.bgP2BuffList = gameManager.bgP2BuffList + "\n" + gameManager.player2Choice;
            if (SceneManager.GetActiveScene().name == "PVP")
            {
                gameManager.BGOutcomeTxt.text = "Player 1 chose: " + gameManager.player1Choice + "\n Player 2 chose: " + gameManager.player2Choice + "\n their choices are different both players get the buffs they wanted";
            }
            else
            {
                gameManager.BGOutcomeTxt.text = "Player 1 chose: " + gameManager.player1Choice + "\n AI chose: " + gameManager.player2Choice + "\n their choices are different the player and AI get the buffs they wanted";
            }
            
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "PVP")
            {
                gameManager.BGOutcomeTxt.text = "Player 1 chose: " + gameManager.player1Choice + "\n Player 2 chose: " + gameManager.player2Choice + "\n their choices are the same neither player gets the buff they wanted";
            }
            else
            {
                gameManager.BGOutcomeTxt.text = "Player 1 chose: " + gameManager.player1Choice + "\n AI chose: " + gameManager.player2Choice + "\n their choices are the same neither the player or AI gets the buff they wanted";
            }
            
        }
        gameManager.bgRoundCount++;
        if (gameManager.bgRoundCount < 4)
        {
            ResetRound();
        }
    }

    private void ResetRound()
    {
        gameManager.p1Ready = false;
        if(SceneManager.GetActiveScene().name=="PVP")
        {
            gameManager.p2Ready = false;
        }
        
        RandomizeChoices();
    }

    private void RandomizeChoices()
    {
        gameManager.player1Choice = RandomChoice();
        gameState.p1ChoiceAI = gameManager.player1Choice;
        if (SceneManager.GetActiveScene().name == "PVP")
        {
            gameManager.player2Choice = RandomChoice();
            gameState.p2ChoiceAI = gameManager.player2Choice;
        }
        Debug.Log("New round starts now!");
    }

    private string RandomChoice()
    {
        int rnd = Random.Range(0, 3);
        switch (rnd)
        {
            case 0: return "Attack";
            case 1: return "Defense";
            case 2: return "Stamina";
            default: return "Attack"; 
        }
    }
    private void AIAutomaticInput()
    {
        if (!gameManager.p2Ready)
        {
            Move bestMove = ai.GetBestMove(gameState);
            gameManager.player2Choice = bestMove.Choice;
            gameManager.p2Ready = true;
            gameState.p2ReadyAI = true;
            Debug.Log("AI chooses " + gameManager.player2Choice);
            CheckPlayersReady();
        }
    }
}

