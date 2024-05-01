using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffGame : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager, assign in Unity Editor

    private void Start()
    {
        RandomizeChoices();
    }

    private void Update()
    {
        Player1Input();
        Player2Input();
    }

    private void Player1Input()
    {
        if (Input.GetKeyDown(KeyCode.A)) { gameManager.player1Choice = "Attack"; }
        if (Input.GetKeyDown(KeyCode.S)) { gameManager.player1Choice = "Defense"; }
        if (Input.GetKeyDown(KeyCode.D)) { gameManager.player1Choice = "Stamina"; }

        if (Input.GetKeyDown(KeyCode.E))
        {
            gameManager.p1Ready = true;
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
            gameManager.BGOutcomeTxt.text = "Player 1 chose: " + gameManager.player1Choice + "\n Player 2 chose: " +gameManager.player2Choice +"\n their choices are different both players get the buffs they wanted";
        }
        else
        {
            gameManager.BGOutcomeTxt.text = "Player 1 chose: " + gameManager.player1Choice + "\n Player 2 chose: " + gameManager.player2Choice + "\n their choices are the same neither player gets the buff they wanted";
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
        gameManager.p2Ready = false;
        RandomizeChoices();
    }

    private void RandomizeChoices()
    {
        gameManager.player1Choice = RandomChoice();
        gameManager.player2Choice = RandomChoice();
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
}

