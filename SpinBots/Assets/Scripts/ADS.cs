using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADS : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager, assign in Unity Editor

    private int player1Score = 0;
    private int player2Score = 0;
    

    private string player1Choice = "Attack";
    private string player2Choice = "Attack";
    

    private void Start()
    {
        StartCoroutine(Player1Turn());
        StartCoroutine(Player2Turn());
    }

    private IEnumerator Player1Turn()
    {
        while (!gameManager.p1Ready)
        {
            if (Input.GetKeyDown(KeyCode.A)) { player1Choice = "Attack"; Debug.Log("Player 1 chooses Attack"); }
            if (Input.GetKeyDown(KeyCode.S)) { player1Choice = "Stamina"; Debug.Log("Player 1 chooses Stamina"); }
            if (Input.GetKeyDown(KeyCode.D)) { player1Choice = "Defense"; Debug.Log("Player 1 chooses Defense"); }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(CountdownForPlayer1()!=null)
                {
                    StopCoroutine(CountdownForPlayer1());
                }
                    
                gameManager.p1Ready = true;
                Debug.Log("Player 1 is ready");
                if (!gameManager.p2Ready)
                {
                    StartCoroutine(CountdownForPlayer2());
                }
                else
                {
                    EvaluateRound();
                }
            }
            yield return null;
        }
    }

    private IEnumerator Player2Turn()
    {
        while (!gameManager.p2Ready)
        {
            if (Input.GetKeyDown(KeyCode.J)) { player2Choice = "Attack"; Debug.Log("Player 2 chooses Attack"); }
            if (Input.GetKeyDown(KeyCode.K)) { player2Choice = "Stamina"; Debug.Log("Player 2 chooses Stamina"); }
            if (Input.GetKeyDown(KeyCode.L)) { player2Choice = "Defense"; Debug.Log("Player 2 chooses Defense"); }

            if (Input.GetKeyDown(KeyCode.O))
            {
                if (CountdownForPlayer2() != null)
                {
                    StopCoroutine(CountdownForPlayer2());
                }
                gameManager.p2Ready = true;
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
            Debug.Log("Player 1 auto-ready after countdown");
            EvaluateRound();
            

        }
    }

    private IEnumerator CountdownForPlayer2()
    {
        float timer = 3000f;
        while (timer > 0 && !gameManager.p2Ready)
        {
            timer --;
            //Debug.Log(timer);
            yield return null;
        }
        if (!gameManager.p2Ready)
        {
            gameManager.p2Ready = true;
            Debug.Log("Player 2 auto-ready after countdown");
            EvaluateRound();
        }
    }

    private void EvaluateRound()
    {
        string winner = "";
        string winningType = "";
        if (player1Choice == player2Choice)
        {
            Debug.Log("Round draw!");
        }
        else if ((player1Choice == "Attack" && player2Choice == "Stamina") ||
                 (player1Choice == "Stamina" && player2Choice == "Defense") ||
                 (player1Choice == "Defense" && player2Choice == "Attack"))
        {
            player1Score++;
            winner = "Player1";
            winningType = player1Choice;
            Debug.Log("Player 1 wins the round with " + player1Choice + "!");
        }
        else
        {
            player2Score++;
            winner = "Player2";
            winningType = player2Choice;
            Debug.Log("Player 2 wins the round with " + player2Choice + "!");
        }

        // Update stats in GameManager
        gameManager.IncreaseStat(winner, winningType);

        gameManager.roundCount++;
        if (gameManager.roundCount < 3)
        {
            ResetRound();
        }
        else
        {
            // Determine overall winner and update stats
            if (player1Score > player2Score)
            {
                winner = "Player1";
            }
            else
            {
                winner = "Player2";
            }
            gameManager.UpdateOverallWinnerStats(winner);
            Debug.Log("Game Over: Player 1 Score: " + player1Score + " - Player 2 Score: " + player2Score);
        }
    }

    private void ResetRound()
    {
        StopAllCoroutines();
        gameManager.p1Ready = false;
        gameManager.p2Ready = false;
        player1Choice = "Attack";
        player2Choice = "Attack";
        Debug.Log("Next round starts now!");
        StartCoroutine(DelayBeforeNextRound());
        
    }
    private IEnumerator DelayBeforeNextRound()
    {
        yield return new WaitForSecondsRealtime(3f);  // Short delay to allow input states to clear
        StartCoroutine(Player1Turn());
        StartCoroutine(Player2Turn());
    }
}
