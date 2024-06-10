using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ADS : MonoBehaviour
{
    public GameManager gameManager;
    private MinimaxAI ai;
    private GameState gameState;


    private void Start()
    {
        
        int rndP1 = Random.Range(0, 3);
        switch (rndP1)
        {
            case 0:
                gameManager.player1Choice = "Attack";
                break;
            case 1:
                gameManager.player1Choice = "Defense";
                break;
            case 2:
                gameManager.player1Choice = "Stamina";
                break;
        }
        if (SceneManager.GetActiveScene().name == "PVP")
        {
            int rndP2 = Random.Range(0, 3);
            switch (rndP2)
            {
                case 0:
                    gameManager.player2Choice = "Attack";
                    break;
                case 1:
                    gameManager.player2Choice = "Defense";
                    break;
                case 2:
                    gameManager.player2Choice = "Stamina";
                    break;
            }
        }
        else
        {
            AIAutomaticInput();
        }
        StartCoroutine(Player1Turn());
        if (SceneManager.GetActiveScene().name == "PVP")
        {
            StartCoroutine(Player2Turn());
        }
        else 
        {
            AIAutomaticInput();
        }

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
    }
    private void Update()
    {
        StartCoroutine(Player1Turn());
        if (SceneManager.GetActiveScene().name == "PVP")
        {
            StartCoroutine(Player2Turn());
        }
        else if (SceneManager.GetActiveScene().name == "PvAI")
        {
            AIAutomaticInput();
        }
    }
    private IEnumerator Player1Turn()
    {
        while (!gameManager.p1Ready)
        {
            if (Input.GetKeyDown(KeyCode.A)) { gameManager.player1Choice = "Attack"; Debug.Log("Player 1 chooses Attack"); }
            if (Input.GetKeyDown(KeyCode.S)) { gameManager.player1Choice = "Defense"; Debug.Log("Player 1 chooses Defense"); }
            if (Input.GetKeyDown(KeyCode.D)) { gameManager.player1Choice = "Stamina"; Debug.Log("Player 1 chooses Stamina"); }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(CountdownForPlayer1()!=null)
                {
                    StopCoroutine(CountdownForPlayer1());
                }
                    
                gameManager.p1Ready = true;
                gameState.p1ReadyAI = true;

                Debug.Log("Player 1 is ready");
                if(SceneManager.GetActiveScene().name == "PVP")
                {
                    if (!gameManager.p2Ready)
                    {
                        StartCoroutine(CountdownForPlayer2());
                    }
                    else
                    {
                        EvaluateRound();
                    }
                }
                else
                {
                    if(gameManager.p2Ready)
                    {
                        EvaluateRound();
                    } 
                }
                
            }
            yield return null;
        }
    }

    private IEnumerator Player2Turn()
    {
        while (!gameManager.p2Ready)
        {
            if (Input.GetKeyDown(KeyCode.J)) { gameManager.player2Choice = "Attack"; Debug.Log("Player 2 chooses Attack"); }
            if (Input.GetKeyDown(KeyCode.K)) { gameManager.player2Choice = "Defense"; Debug.Log("Player 2 chooses Defense"); }
            if (Input.GetKeyDown(KeyCode.L)) { gameManager.player2Choice = "Stamina"; Debug.Log("Player 2 chooses Stamina"); }

            if (Input.GetKeyDown(KeyCode.O))
            {
                if (CountdownForPlayer2() != null)
                {
                    StopCoroutine(CountdownForPlayer2());
                }
                gameManager.p2Ready = true;
                gameState.p2ReadyAI = true;
                
                Debug.Log("Player 2 is ready");
                if (!gameManager.p1Ready)
                {
                    StartCoroutine(CountdownForPlayer1());
                }
                else
                {
                    EvaluateRound();
                }
            }
            yield return null;
        }
    }

    private IEnumerator CountdownForPlayer1()
    {
        float timer = 3000f;
        while (timer > 0 && !gameManager.p1Ready)
        {
            timer--;
            //Debug.Log(timer);
            yield return null;
        }
        if (!gameManager.p1Ready)
        {
            gameManager.p1Ready = true;
            gameState.p1ReadyAI = true;
            Debug.Log("Player 1 auto-ready after countdown");
            EvaluateRound();
            

        }
    }

    private IEnumerator CountdownForPlayer2()
    {
        float timer = 3000f;
        while (timer > 0 && !gameManager.p2Ready)
        {
            timer--;
            //Debug.Log(timer);
            yield return null;
        }
        if (!gameManager.p2Ready)
        {
            gameManager.p2Ready = true;
            gameState.p2ReadyAI = true;
            Debug.Log("Player 2 auto-ready after countdown");
            EvaluateRound();
        }
    }

    private void EvaluateRound()
    {
        
        if (gameManager.player1Choice == gameManager.player2Choice)
        {
            gameManager.ADSWinner.text = "Round " + gameManager.roundCount + " is a draw";
            Debug.Log("Round draw!");
        }
        else if ((gameManager.player1Choice == "Attack" && gameManager.player2Choice == "Stamina") ||
                 (gameManager.player1Choice == "Stamina" && gameManager.player2Choice == "Defense") ||
                 (gameManager.player1Choice == "Defense" && gameManager.player2Choice == "Attack"))
        {
            gameManager.player1Score++;
            gameManager.winner = "Player1";
            gameManager.winningType = gameManager.player1Choice;
            gameManager.ADSWinner.text = "The winner for round " + gameManager.roundCount + " is:\n" + gameManager.winner + " with " + gameManager.winningType;
            Debug.Log("Player 1 wins the round with " + gameManager.player1Choice + "!");
        }
        else
        {
            gameManager.player2Score++;
            if(SceneManager.GetActiveScene().name=="PVP")
            {
                gameManager.winner = "Player2";
            }
            else
            {
                gameManager.winner = "AI";
            }
            
            gameManager.winningType = gameManager.player2Choice;
            gameManager.ADSWinner.text = "The winner for round " + gameManager.roundCount + " is:\n" + gameManager.winner + " with " + gameManager.winningType;
            Debug.Log("Player 2 wins the round with " + gameManager.player2Choice + "!");
        }

        
        gameManager.IncreaseStat(gameManager.winner, gameManager.winningType);

        gameManager.roundCount++;
        if (gameManager.roundCount < 4)
        {
            ResetRound();
        }
        else
        {
            
            if (gameManager.player1Score > gameManager.player2Score)
            {
                gameManager.winner = "Player1";
            }
            else
            {

                if (SceneManager.GetActiveScene().name == "PVP")
                {
                    gameManager.winner = "Player2";
                }
                else
                {
                    gameManager.winner = "AI";
                }
            }
            gameManager.UpdateOverallWinnerStats(gameManager.winner);
            Debug.Log("Game Over: Player 1 Score: " + gameManager.player1Score + " - Player 2 Score: " + gameManager.player2Score);
        }
    }

    private void ResetRound()
    {
        StopAllCoroutines();
        gameManager.p1Ready = false;
        gameManager.p2Ready = false;
        gameState.p1ReadyAI = false;
        gameState.p2ReadyAI = false;
        int rndP1 = Random.Range(0, 3);
        switch (rndP1)
        {
            case 0:
                gameManager.player1Choice = "Attack";
                break;
            case 1:
                gameManager.player1Choice = "Defense";
                break;
            case 2:
                gameManager.player1Choice = "Stamina";
                break;
        }
        gameState.p1ChoiceAI = gameManager.player1Choice;
        if (SceneManager.GetActiveScene().name == "PVP")
        {
            int rndP2 = Random.Range(0, 3);
            switch (rndP2)
            {
                case 0:
                    gameManager.player2Choice = "Attack";
                    break;
                case 1:
                    gameManager.player2Choice = "Defense";
                    break;
                case 2:
                    gameManager.player2Choice = "Stamina";
                    break;
            }
        }
        else if (SceneManager.GetActiveScene().name == "PvAI")
        {
            AIAutomaticInput();
        }
        Debug.Log("Next round starts now!");
        StartCoroutine(DelayBeforeNextRound());
        
    }
    private IEnumerator DelayBeforeNextRound()
    {
        yield return new WaitForSecondsRealtime(3f);  
        StartCoroutine(Player1Turn());
        if (SceneManager.GetActiveScene().name == "PVP")
        {
            StartCoroutine(Player2Turn());
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

    private void CheckPlayersReady()
    {
        if (gameManager.p1Ready && gameManager.p2Ready)
        {
            EvaluateRound();
        }
    }
}
