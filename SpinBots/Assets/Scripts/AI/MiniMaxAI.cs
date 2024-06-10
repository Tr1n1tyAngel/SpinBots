using System;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxAI
{
    private int depth = 3; // Default depth

    public void SetDepth(int newDepth)
    {
        depth = newDepth;
    }
    public Move GetBestMove(GameState initialState)
    {
        
        bool maximizingPlayer = true; // AI starts as maximizing player

        return Minimax(initialState, depth, maximizingPlayer).Item2;
    }

    private Tuple<int, Move> Minimax(GameState state, int depth, bool maximizingPlayer)
    {
        if (depth == 0 || state.IsTerminal())
        {
            return new Tuple<int, Move>(Evaluate(state), null);
        }

        List<Move> possibleMoves = GetPossibleMoves(state);

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            Move bestMove = null;

            foreach (Move move in possibleMoves)
            {
                GameState newState = ApplyMove(state.Clone(), move);
                int eval = Minimax(newState, depth - 1, false).Item1;

                if (eval > maxEval)
                {
                    maxEval = eval;
                    bestMove = move;
                }
            }

            return new Tuple<int, Move>(maxEval, bestMove);
        }
        else
        {
            int minEval = int.MaxValue;
            Move bestMove = null;

            foreach (Move move in possibleMoves)
            {
                GameState newState = ApplyMove(state.Clone(), move);
                int eval = Minimax(newState, depth - 1, true).Item1;

                if (eval < minEval)
                {
                    minEval = eval;
                    bestMove = move;
                }
            }

            return new Tuple<int, Move>(minEval, bestMove);
        }
    }

    private int Evaluate(GameState state)
    {
        int score = 0;

        // Evaluate each stat separately and consider type advantages/disadvantages
        score += EvaluateStat(state.p2AttackStatAI, state.p1AttackStatAI, "Attack", "Stamina");
        score += EvaluateStat(state.p2DefenseStatAI, state.p1DefenseStatAI, "Defense", "Attack");
        score += EvaluateStat(state.p2StaminaStatAI, state.p1StaminaStatAI, "Stamina", "Defense");

        return score;
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
    private List<Move> GetPossibleMoves(GameState state)
    {
        List<Move> moves = new List<Move>();

        if (!state.p2BotSelectedAI)
        {
            moves.Add(new Move("SpinBotSelection", "Attack"));
            moves.Add(new Move("SpinBotSelection", "Defense"));
            moves.Add(new Move("SpinBotSelection", "Stamina"));
        }

        if (state.roundCountAI < 4)
        {
            moves.Add(new Move("ADS", "Attack"));
            moves.Add(new Move("ADS", "Defense"));
            moves.Add(new Move("ADS", "Stamina"));
        }
        if (state.bgRoundCountAI < 4)
        {
            moves.Add(new Move("Buff", "Attack"));
            moves.Add(new Move("Buff", "Defense"));
            moves.Add(new Move("Buff", "Stamina"));
        }

        return moves;
    }


    private GameState ApplyMove(GameState state, Move move)
    {
        switch (move.Type)
        {
            case "SpinBotSelection":
                float playerHighestStat = Mathf.Max(state.p1AttackStatAI, state.p1DefenseStatAI, state.p1StaminaStatAI);
                if (move.Choice == "Attack")
                {
                    state.p2AttackStatAI = 8;
                    state.p2DefenseStatAI = 3;
                    state.p2StaminaStatAI = 4;
                }
                else if (move.Choice == "Defense")
                {
                    state.p2AttackStatAI = 3;
                    state.p2DefenseStatAI = 8;
                    state.p2StaminaStatAI = 4;
                }
                else if (move.Choice == "Stamina")
                {
                    state.p2AttackStatAI = 4;
                    state.p2DefenseStatAI = 3;
                    state.p2StaminaStatAI = 8;
                }
                state.p2BotSelectedAI = true;
                break;

            case "ADS":
                state.p2ChoiceAI = move.Choice;

                if (state.p2ChoiceAI == state.p1ChoiceAI)
                {
                    // It's a draw, no score change
                }
                else if ((state.p2ChoiceAI == "Attack" && state.p1ChoiceAI == "Stamina") ||
                         (state.p2ChoiceAI == "Stamina" && state.p1ChoiceAI == "Defense") ||
                         (state.p2ChoiceAI == "Defense" && state.p1ChoiceAI == "Attack"))
                {
                    state.player2ScoreAI++;
                }
                else
                {
                    state.player1ScoreAI++;
                }

                state.roundCountAI++;
                break;
            case "Buff":
                state.p2ChoiceAI = move.Choice;

                if (state.p2ChoiceAI != state.p1ChoiceAI)
                {
                    // Both players get their selected buffs
                    state.p2AttackStatAI += (move.Choice == "Attack") ? 1 : 0;
                    state.p2DefenseStatAI += (move.Choice == "Defense") ? 1 : 0;
                    state.p2StaminaStatAI += (move.Choice == "Stamina") ? 1 : 0;
                }

                state.bgRoundCountAI++;
                break;
        }

        return state;
    }
}

