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
        if (p2AttackStatAI + p2DefenseStatAI + p2StaminaStatAI > p1AttackStatAI + p1DefenseStatAI + p1StaminaStatAI)
        {
            return 1; // AI wins
        }
        else if (p2AttackStatAI + p2DefenseStatAI + p2StaminaStatAI < p1AttackStatAI + p1DefenseStatAI + p1StaminaStatAI)
        {
            return 0; // Player wins
        }
        else
        {
            return -1; // Draw
        }
    }
}

