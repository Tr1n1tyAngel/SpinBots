using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameState
{
        
        public float p1AttackStatAI;
        public float p1DefenseStatAI;
        public float p1StaminaStatAI;
        public float p2AttackStatAI;
        public float p2DefenseStatAI;
        public float p2StaminaStatAI;

        public bool p1BotSelectedAI;
        public bool p2BotSelectedAI;


        public string p1ChoiceAI;
        public string p2ChoiceAI;

        public bool gameOverAI;

        public bool p1ReadyAI;
        public bool p2ReadyAI;
        public int roundCountAI;
        public int bgRoundCountAI;
        public int player1ScoreAI;
        public int player2ScoreAI;



    public GameState Clone()
        {
            return (GameState)this.MemberwiseClone();
        }

        public bool IsTerminal()
        {
            
            return gameOverAI; 
        }

    public int GetResult()
    {
        int score = 0;

        // Evaluate each stat separately and consider type advantages/disadvantages
        score += EvaluateStat(p2AttackStatAI, p1AttackStatAI, "Attack", "Stamina");
        score += EvaluateStat(p2DefenseStatAI, p1DefenseStatAI, "Defense", "Attack");
        score += EvaluateStat(p2StaminaStatAI, p1StaminaStatAI, "Stamina", "Defense");

        // Determine result based on score
        if (score > 0)
        {
            return 1; // AI wins
        }
        else if (score < 0)
        {
            return 0; // Player wins
        }
        else
        {
            return -1; // Draw
        }
    }

    private int EvaluateStat(float aiStat, float playerStat, string aiType, string playerType)
    {
        float advantage = 0;

        if (aiStat > playerStat) advantage += 1;
        if (aiStat < playerStat) advantage -= 1;

        if ((aiType == "Attack" && playerType == "Stamina") ||
            (aiType == "Defense" && playerType == "Attack") ||
            (aiType == "Stamina" && playerType == "Defense"))
        {
            advantage += 0.5f;
        }
        else if ((aiType == "Attack" && playerType == "Defense") ||
                 (aiType == "Defense" && playerType == "Stamina") ||
                 (aiType == "Stamina" && playerType == "Attack"))
        {
            advantage -= 0.5f;
        }

        return Mathf.RoundToInt(advantage);
    }
}


