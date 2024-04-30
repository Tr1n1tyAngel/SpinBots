using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ADS : MonoBehaviour
{
    public GameManager gameManager; 
    

    private void Start()
    {
        int rndP1 = Random.Range(0, 3);
        switch(rndP1)
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
        StartCoroutine(Player1Turn());
        StartCoroutine(Player2Turn());
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
            timer--;
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
            gameManager.winner = "Player2";
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
                gameManager.winner = "Player2";
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
        Debug.Log("Next round starts now!");
        StartCoroutine(DelayBeforeNextRound());
        
    }
    private IEnumerator DelayBeforeNextRound()
    {
        yield return new WaitForSecondsRealtime(3f);  
        StartCoroutine(Player1Turn());
        StartCoroutine(Player2Turn());
    }
}
